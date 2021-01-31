using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Charlotte.Commons;
using Charlotte.Tests;
using System.Threading;

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

		private static void ProductMain(ArgsReader ar)
		{
			try
			{
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
			string[] rPaths = Enumerable.Concat(
				Directory.GetDirectories(targetDir, "*", SearchOption.AllDirectories),
				Directory.GetFiles(targetDir, "*", SearchOption.AllDirectories)
				)
				.ToArray();
			Array.Sort(rPaths, SCommon.CompIgnoreCase);
			string[] wPaths = rPaths.Select(path => SCommon.ChangeRoot(path, targetDir, destDir)).ToArray();

			// rPaths, wPaths の並びの対応を維持すること。

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

		private static string GetDestDir(DateTime dt, string targetPath)
		{
			string destDir = Path.Combine(Consts.SNAPSHOT_DIR, dt.ToString("yyyyMMddHHmmss") + " " + targetPath.Replace(':', '$').Replace('\\', '$'));

			Console.WriteLine("destDir: " + destDir); // cout

			if (Directory.Exists(destDir))
				throw new Exception("already exists destDir"); // 恐らく不運な衝突 -- やり直せば良いはず。

			return destDir;
		}

		private static void CheckDestPath(string destPath)
		{
			int len = SCommon.ENCODING_SJIS.GetByteCount(destPath);

			if (Consts.PATH_MAX < len)
				throw new Exception("長すぎる出力パス：" + destPath + " (" + len + ")");
		}
	}
}
