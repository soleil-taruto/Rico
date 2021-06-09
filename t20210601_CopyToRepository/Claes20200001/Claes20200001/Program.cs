using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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
				// -- choose one --

				new Test0001().Test01();
				//new Test0001().Test02();
				//new Test0001().Test03();

				// --
			}
			else
			{
				this.Main3(ar);
			}
		}

		private void Main3(ArgsReader ar)
		{
			string rDir = ar.NextArg();
			string wDir = ar.NextArg();

			rDir = SCommon.MakeFullPath(rDir);
			wDir = SCommon.MakeFullPath(wDir);

			Console.WriteLine("< " + rDir); // cout
			Console.WriteLine("> " + wDir); // cout

			if (!Directory.Exists(rDir))
				throw new Exception("no rDir");

			if (!Directory.Exists(wDir))
				throw new Exception("no wDir");

			if (this.IsLikeARepository(rDir))
				throw new Exception("rDir is like a repository");

			if (!this.IsRepository(wDir))
				throw new Exception("wDir is not repository");

			this.CopyToRepository(rDir, wDir);

			Console.WriteLine("OK!"); // cout
		}

		private bool IsLikeARepository(string dir)
		{
			string gitDir = Path.Combine(dir, Consts.GIT_LOCAL_DIR);
			string gitAttributesFile = Path.Combine(dir, Consts.GIT_ATTRIBUTES_LOCAL_FILE);

			return
				Directory.Exists(gitDir) ||
				File.Exists(gitAttributesFile);
		}

		private bool IsRepository(string dir)
		{
			string gitDir = Path.Combine(dir, Consts.GIT_LOCAL_DIR);
			string gitAttributesFile = Path.Combine(dir, Consts.GIT_ATTRIBUTES_LOCAL_FILE);

			return
				Directory.Exists(gitDir) &&
				File.Exists(gitAttributesFile);
		}

		public void CopyToRepository(string rDir, string wDir)
		{
			string[] rSubDirs = Directory.GetDirectories(rDir);
			string[] wSubDirs = Directory.GetDirectories(wDir);
			string[] rFiles = Directory.GetFiles(rDir);
			string[] wFiles = Directory.GetFiles(wDir);

			Comparison<string> compLocalPath = (a, b) => SCommon.Comp(Path.GetFileName(a), Path.GetFileName(b));

			Func<string, string, bool> accepter = (rPath, wPath) =>
			{
				string rLocalPath = Path.GetFileName(rPath);

				if (SCommon.EqualsIgnoreCase(rLocalPath, "dat") && Directory.Exists(rPath))
				{
					SCommon.CreateDir(wPath);
					File.WriteAllLines(Path.Combine(wPath, "____files.txt"), GetFileInfoLines(rPath), Encoding.UTF8);
					return false;
				}
				string rExt = Path.GetExtension(rPath);

				return
					!SCommon.EqualsIgnoreCase(rExt, ".exe") &&
					!SCommon.EqualsIgnoreCase(rExt, ".obj");
			};

			using (FolderUpdateMonitor fum = new FolderUpdateMonitor())
			{
				foreach (Common.PairInfo<string> pair in Common.Merge(rSubDirs, wSubDirs, compLocalPath))
				{
					string rSubDir = pair.A;
					string wSubDir = pair.B;

					if (rSubDir == null)
					{
						if (SCommon.EqualsIgnoreCase(Path.GetFileName(wSubDir), Consts.GIT_LOCAL_DIR))
						{
							// Skip
						}
						else
						{
							SCommon.DeletePath(wSubDir);
						}
					}
					else
					{
						bool updated = fum.IsUpdated(rSubDir);

						Console.WriteLine(rSubDir + " is" + (updated ? "" : " not") + " updated."); // cout

						if (wSubDir == null)
						{
							wSubDir = Path.Combine(wDir, Path.GetFileName(rSubDir));
							updated = true;
						}
						if (updated)
						{
							SCommon.DeletePath(wSubDir); // 大文字・小文字が変わっただけの場合を想定して、ここで削除する必要がある。
							SCommon.CopyDir(rSubDir, wSubDir, accepter);
						}
					}
				}
			}

			foreach (Common.PairInfo<string> pair in Common.Merge(rFiles, wFiles, compLocalPath))
			{
				string rFile = pair.A;
				string wFile = pair.B;

				if (rFile == null)
				{
					if (SCommon.EqualsIgnoreCase(Path.GetFileName(wFile), Consts.GIT_ATTRIBUTES_LOCAL_FILE))
					{
						// Skip
					}
					else if (SCommon.EqualsIgnoreCase(Path.GetFileName(wFile), Consts.LICENSE_LOCAL_FILE))
					{
						// Skip
					}
					else
					{
						SCommon.DeletePath(wFile);
					}
				}
				else
				{
					if (wFile == null)
						wFile = Path.Combine(wDir, Path.GetFileName(rFile));

					if (accepter(rFile, wFile))
					{
						SCommon.DeletePath(wFile); // 大文字・小文字が変わっただけの場合を想定して、ここで削除する必要がある。
						File.Copy(rFile, wFile);
					}
				}
			}
		}

		private static IEnumerable<string> GetFileInfoLines(string targDir)
		{
			yield return "CREATE TIME              | UPDATE TIME              | HASH (MD5)                       | SIZE      | PATH";
			yield return "-------------------------+--------------------------+----------------------------------+-----------+----------------------------------------";

			string[] dirs = Directory.GetDirectories(targDir, "*", SearchOption.AllDirectories)
				.Select(dir => SCommon.MakeFullPath(dir))
				.ToArray();

			Array.Sort(dirs, SCommon.CompIgnoreCase);

			foreach (string dir in dirs)
			{
				DirectoryInfo info = new DirectoryInfo(dir);

				yield return GetFileInfoLine(SCommon.ChangeRoot(dir, targDir), info.CreationTime, info.LastWriteTime, null, -1L);
			}

			string[] files = Directory.GetFiles(targDir, "*", SearchOption.AllDirectories)
				.Select(file => SCommon.MakeFullPath(file))
				.ToArray();

			Array.Sort(files, SCommon.CompIgnoreCase);

			string[] hashes;
			try
			{
				ProcMain.WriteLog("GetHashes.1"); // cout
				hashes = Common.GetHashes(files);
				ProcMain.WriteLog("GetHashes.2"); // cout
			}
			catch (Exception ex)
			{
				ProcMain.WriteLog("ex: " + ex + " (処理続行)"); // cout
				hashes = files.Select(file => (string)null).ToArray(); // 代替ハッシュリスト == 全部 null
				ProcMain.WriteLog("GetHashes.3"); // cout
			}

			for (int index = 0; index < files.Length; index++)
			{
				string file = files[index];
				string hash = hashes[index];

				FileInfo info = new FileInfo(file);

				yield return GetFileInfoLine(SCommon.ChangeRoot(file, targDir), info.CreationTime, info.LastWriteTime, hash, info.Length);
			}
			yield return "-------------------------+--------------------------+----------------------------------+-----------+----------------------------------------";
		}

		private static string GetFileInfoLine(string path, DateTime createTime, DateTime updateTime, string hash, long size)
		{
			return string.Join(
				" | ",
				new SCommon.SimpleDateTime(createTime),
				new SCommon.SimpleDateTime(updateTime),
				hash == null ? "--------------------------------" : hash,
				size == -1L ? "---------" : Common.LZPad("" + size, 9, " "),
				path
				);
		}
	}
}
