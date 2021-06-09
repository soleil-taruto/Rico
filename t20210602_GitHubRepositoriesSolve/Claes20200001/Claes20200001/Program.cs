using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Charlotte.Commons;
using Charlotte.Tests;

namespace Charlotte
{
	class Program
	{
		static void Main(string[] args)
		{
			ProcMain.CUIMain(new Program().Main2);
		}

		private void Main2(ArgsReader ar)
		{
			if (ProcMain.DEBUG)
			{
				TestMain(); // テスト
			}
			else
			{
				ProductMain(); // 本番
			}
		}

		private void TestMain()
		{
			// -- choose one --

			ProductMain();
			//new Test0001().Test01();
			//new Test0001().Test02();
			//new Test0001().Test03();

			// --

			Console.WriteLine("Press ENTER key.");
			Console.ReadLine();
		}

		private void ProductMain()
		{
			if (!Directory.Exists(Consts.REPOSITORIES_ROOT_DIR))
				throw new Exception("リポジトリのルート・ディレクトリが見つかりません。");

			foreach (string dir in Directory.GetDirectories(Consts.REPOSITORIES_ROOT_DIR))
				Solve(dir);

			//CompactRRD(); // やたら時間掛かることがあるので、抑止
		}

		private void CompactRRD()
		{
			Console.WriteLine("圧縮しています...");
			SCommon.Batch(new string[]
			{
				"Compact.exe /C /S:\"" + Consts.REPOSITORIES_ROOT_DIR + "\"",
			},
			"",
			SCommon.StartProcessWindowStyle_e.MINIMIZED
			);
			Console.WriteLine("圧縮完了");
		}

		private void Solve(string dir)
		{
			Console.WriteLine("dir: " + dir); // cout
			Console.WriteLine("*1"); // cout
			SolveForFactory(dir);
			Console.WriteLine("*2"); // cout
			SolveGameResource(dir);
			Console.WriteLine("*3"); // cout
			SolveNonAsciiCharactersPaths(dir);
			Console.WriteLine("*4"); // cout
			SolveEmptyFolders(dir);
			Console.WriteLine("*5"); // cout
			SolveTextEncoding(dir);
			Console.WriteLine("*6"); // cout
		}

		private void SFVS2019_Mask(string file)
		{
			const string FILE_SUFFIX = "_ghrs-vs2019-ignore.txt";
			const string MASKED_TEXT = "//// ghrs-vs2019-ignore ////";

			if (SCommon.EndsWithIgnoreCase(file, FILE_SUFFIX)) // ? マスク済み
				return;

			SCommon.DeletePath(file);
			File.WriteAllText(file + FILE_SUFFIX, MASKED_TEXT);
		}

		private void SolveNonAsciiCharactersPaths(string dir)
		{
			foreach (string subDir in Directory.GetDirectories(dir))
			{
				if (SCommon.EqualsIgnoreCase(Path.GetFileName(subDir), ".git"))
					continue;

				SolveNonAsciiCharactersPaths(subDir);
				SNACP_Path(subDir);
			}
			foreach (string file in Directory.GetFiles(dir))
			{
				SNACP_Path(file);
			}
		}

		private void SNACP_Path(string path)
		{
			string localPath = Path.GetFileName(path);
			string localPathNew = ToAsciiCharactersLocalPath(localPath);

			if (!SCommon.EqualsIgnoreCase(localPath, localPathNew))
			{
				string pathNew = Path.Combine(Path.GetDirectoryName(path), localPathNew);

				if (File.Exists(pathNew) || Directory.Exists(pathNew))
					throw new Exception("変更後のパス名は既に存在します。");

				if (File.Exists(path))
					File.Move(path, pathNew);
				else
					Directory.Move(path, pathNew);
			}
		}

		private string ToAsciiCharactersLocalPath(string localPath)
		{
			StringBuilder buff = new StringBuilder();

			foreach (char chr in localPath)
			{
				if (chr < 0x100)
					buff.Append(chr);
				else
					buff.Append(((ushort)chr).ToString("x4"));
			}
			return buff.ToString();
		}

		private void SolveEmptyFolders(string dir)
		{
			if (
				Directory.GetDirectories(dir).Length == 0 &&
				Directory.GetFiles(dir).Length == 0
				)
			{
				string outFile = Path.Combine(dir, "____EMPTY____");

				File.WriteAllBytes(outFile, SCommon.EMPTY_BYTES);

				return;
			}

			foreach (string subDir in Directory.GetDirectories(dir))
			{
				if (SCommon.EqualsIgnoreCase(Path.GetFileName(subDir), ".git"))
					continue;

				SolveEmptyFolders(subDir);
			}
		}

		private void SolveForFactory(string dir)
		{
			foreach (string file in Common.GetRepositoryFiles(dir))
			{
				string lwrExt = Path.GetExtension(file).ToLower();

				if (
					lwrExt == ".exe" ||
					lwrExt == ".obj"
					)
					SCommon.DeletePath(file);
			}
			foreach (string file in Common.GetRepositoryFiles(dir))
			{
				if (
					SCommon.ContainsIgnoreCase(file, "\\tmp\\") ||
					SCommon.ContainsIgnoreCase(file, "\\tmp_")
					)
					SCommon.DeletePath(file);
			}
		}

		private void SolveGameResource(string dir)
		{
			foreach (string file in Common.GetRepositoryFiles(dir))
			{
				if (
					SCommon.ContainsIgnoreCase(file, "\\Games\\") && // ? 名前空間 Games の配下
					SCommon.EndsWithIgnoreCase(file, ".cs")
					)
					SGR_MaskLiteralString(file);
			}
		}

		private void SGR_MaskLiteralString(string file)
		{
			const string PTN_INCLUDE_RESOURCE = "_#Include_Resource";

			if (Common.Contains(File.ReadAllBytes(file), Encoding.UTF8.GetBytes(PTN_INCLUDE_RESOURCE)))
			{
				string[] lines = File.ReadAllLines(file, Encoding.UTF8);

				for (int index = 0; index < lines.Length; index++)
				{
					string line = lines[index];

					if (!line.Trim().StartsWith("//") && line.Contains('"')) // ? not コメント行 && リテラル文字列 // HACK: 判定_雑
					{
						line = string.Join("", line.Select(chr => chr < 0x100 ? "" + chr : "\\u" + ((int)chr).ToString("x4")));
						lines[index] = line;
					}
				}
				File.WriteAllLines(file, lines, Encoding.UTF8);
			}
		}

		private void SolveTextEncoding(string dir)
		{
			foreach (string file in Common.GetRepositoryFiles(dir))
			{
				if (SCommon.EqualsIgnoreCase(Path.GetExtension(file), ".txt"))
				{
					byte[] bText = File.ReadAllBytes(file);

					if (
						SCommon.Comp(SCommon.ENCODING_SJIS.GetBytes(SCommon.ToJString(bText, true, true, true, true).Replace("\n", "\r\n")), bText) == 0 && // ? bText == SJIS(CP932)
						SCommon.Comp(SCommon.ENCODING_SJIS.GetBytes(SCommon.ToJString(bText, false, true, true, true).Replace("\n", "\r\n")), bText) != 0 // ? bText != ASCII
						)
					{
						byte[] bTextNew = Common.PutUTF8Bom(Encoding.UTF8.GetBytes(SCommon.ENCODING_SJIS.GetString(bText))); // SJIS -> UTF-8

						File.WriteAllBytes(file, bTextNew);
					}
				}
			}
		}
	}
}
