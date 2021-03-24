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

			this.ClearRepository(wDir);
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

		private void ClearRepository(string dir)
		{
			foreach (string subDir in Directory.GetDirectories(dir))
			{
				if (SCommon.EqualsIgnoreCase(Path.GetFileName(subDir), Consts.GIT_LOCAL_DIR))
				{
					// Skip
				}
				else
				{
					SCommon.DeletePath(subDir);
				}
			}
			foreach (string file in Directory.GetFiles(dir))
			{
				if (SCommon.EqualsIgnoreCase(Path.GetFileName(file), Consts.GIT_ATTRIBUTES_LOCAL_FILE))
				{
					// Skip
				}
				else if (SCommon.EqualsIgnoreCase(Path.GetFileName(file), Consts.LICENSE_LOCAL_FILE))
				{
					// Skip
				}
				else
				{
					SCommon.DeletePath(file);
				}
			}
		}

		public void CopyToRepository(string rDir, string wDir)
		{
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
					!SCommon.EqualsIgnoreCase(rExt, ".obj"); //&&
				//!SCommon.EqualsIgnoreCase(rLocalPath, "desktop.ini"); // フォルダのアイコンを変更していることがある。
			};

			foreach (string rSubDir in Directory.GetDirectories(rDir))
			{
				SCommon.CopyDir(rSubDir, Path.Combine(wDir, Path.GetFileName(rSubDir)), accepter);
			}
			foreach (string rFile in Directory.GetFiles(rDir))
			{
				string wFile = Path.Combine(wDir, Path.GetFileName(rFile));

				if (accepter(rFile, wFile))
					File.Copy(rFile, wFile);
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

			ProcMain.WriteLog("GetHashes.1"); // cout
			string[] hashes = Common.GetHashes(files);
			ProcMain.WriteLog("GetHashes.2"); // cout

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
