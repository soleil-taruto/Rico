using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using Charlotte.Commons;
using Charlotte.Tests;

namespace Charlotte
{
	class Program
	{
		static void Main(string[] args)
		{
			ProcMain.CUIMain(Main2);
		}

		private static void Main2(ArgsReader ar)
		{
			if (ar.ArgIs("//D"))
			{
				TestMain(); // テスト
			}
			else
			{
				ProductMain(ar); // 本番
			}
		}

		private static void TestMain()
		{
			// -- choose one --

			new Test0001().Test01();
			//new Test0001().Test02();
			//new Test0001().Test03();

			// --

			Console.WriteLine("Press ENTER to exit.");
			Console.ReadLine();
		}

		private static bool ManualCopyMode = false;
		private static bool IgnoreSubDirFlag = false;

		private static void ProductMain(ArgsReader ar)
		{
			try
			{
				if (ar.ArgIs("/M"))
				{
					ManualCopyMode = true;
				}
				if (ar.ArgIs("/-S"))
				{
					IgnoreSubDirFlag = true;
				}
				Snapshot(ar.NextArg());
			}
			catch (Exception ex)
			{
				// パスが長すぎる場合など有り得る。
				// -- ちゃんと気付けるようにメッセージダイアログを出す。
				//
				MessageBox.Show("" + ex, "スナップショットに失敗しました", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		private static void Snapshot(string targetPath)
		{
			if (!Directory.Exists(Consts.SNAPSHOT_DIR))
				throw new Exception("no SNAPSHOT_DIR");

			targetPath = SCommon.MakeFullPath(targetPath);

			try
			{
				TrySnapshot(targetPath);
			}
			catch (BadDestPath_Exception)
			{
				ShortDestDirFlag = true;
				TrySnapshot(targetPath);
			}
		}

		private static void TrySnapshot(string targetPath)
		{
			if (File.Exists(targetPath))
			{
				Snapshot_File(targetPath);
			}
			else if (Directory.Exists(targetPath))
			{
				Snapshot_Dir(targetPath);
			}
			else
			{
				throw new Exception("targetPath is not file or directory");
			}

			if (ShortDestDirFlag)
			{
				string tipsFile = Path.Combine(LastDestDir, Consts.TIPS_LOCAL_FILE);

				File.WriteAllLines(
					tipsFile,
					new string[]
					{
						"targetPath=" + targetPath,
					},
					Encoding.UTF8
					);
			}

			SCommon.Batch(new string[] { "START \"\" \"" + LastDestDir + "\"" });
		}

		private static void Snapshot_File(string targetFile)
		{
			string destDir = GetDestDir(DateTime.Now, targetFile);
			string destFile = Path.Combine(destDir, Path.GetFileName(targetFile));

			CheckDestPath(destFile);

			SCommon.CreateDir(destDir);
			File.Copy(targetFile, destFile);
		}

		private static void Snapshot_Dir(string targetDir)
		{
			string destDir = GetDestDir(DateTime.Now, targetDir);
			string[] rPaths;

			if (ManualCopyMode)
			{
				rPaths = new string[0];
			}
			else if (IgnoreSubDirFlag)
			{
				rPaths = Directory.GetFiles(targetDir);
			}
			else
			{
				rPaths = Enumerable.Concat(
					Directory.GetDirectories(targetDir, "*", SearchOption.AllDirectories),
					Directory.GetFiles(targetDir, "*", SearchOption.AllDirectories)
					)
					.ToArray();
			}
			Array.Sort(rPaths, SCommon.CompIgnoreCase);
			string[] wPaths = rPaths.Select(path => SCommon.ChangeRoot(path, targetDir, destDir)).ToArray();

			// rPaths, wPaths の並びの対応を維持すること。

			// 今のところ長さしかチェックしていないので、最も長いパスのみ CheckDestPath に渡す。
			if (1 <= wPaths.Length)
			{
				int longestWPathLen = wPaths.Select(path => path.Length).Max();
				string longestWPath = wPaths.First(path => path.Length == longestWPathLen);

				CheckDestPath(longestWPath);
			}

			SCommon.CreateDir(destDir);

			for (int index = 0; index < rPaths.Length; index++)
			{
				string rPath = rPaths[index];
				string wPath = wPaths[index];

				Console.WriteLine("< " + rPath); // cout
				Console.WriteLine("> " + wPath); // cout

				if (File.Exists(rPath))
				{
					File.Copy(rPath, wPath);
				}
				else if (Directory.Exists(rPath))
				{
					SCommon.CreateDir(wPath);
				}
				else
				{
					throw null; // 想定外
				}
			}
		}

		private static bool ShortDestDirFlag = false;
		private static string LastDestDir = null;

		private static string GetDestDir(DateTime dt, string targetPath)
		{
			string destLocalDir;

			if (ShortDestDirFlag)
				destLocalDir = dt.ToString("yyyyMMddHHmmss");
			else
				destLocalDir = dt.ToString("yyyyMMddHHmmss") + " " + targetPath.Replace(':', '$').Replace('\\', '$');

			string destDir = Path.Combine(Consts.SNAPSHOT_DIR, destLocalDir);

			Console.WriteLine("destDir: " + destDir); // cout

			if (Directory.Exists(destDir))
				throw new Exception("already exists destDir"); // 恐らく不運な衝突 -- やり直せば良いはず。

			LastDestDir = destDir;
			return destDir;
		}

		private class BadDestPath_Exception : Exception
		{ }

		private static void CheckDestPath(string destPath)
		{
			int len = SCommon.ENCODING_SJIS.GetByteCount(destPath);

			if (Consts.PATH_MAX < len)
				throw new Exception("長すぎる出力パス：" + destPath + " (" + len + ")");
		}
	}
}
