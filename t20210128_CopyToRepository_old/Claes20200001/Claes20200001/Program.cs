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
			if (ar.ArgIs("//D"))
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
			foreach (string path in this.GetClearRepositoryEntries(dir).ProgressBar("Clear"))
			{
				SCommon.DeletePath(path);
			}
		}

		private List<string> GetClearRepositoryEntries(string dir)
		{
			List<string> dest = new List<string>();
			this.CollectClearRepositoryEntries(dir, dest);
			return dest;
		}

		private void CollectClearRepositoryEntries(string dir, List<string> dest)
		{
			foreach (string subDir in Directory.GetDirectories(dir))
			{
				if (SCommon.EqualsIgnoreCase(Path.GetFileName(subDir), Consts.GIT_LOCAL_DIR))
				{
					// Skip
				}
				else
				{
					this.CollectDeleteDirectoryEntries(subDir, dest);
				}
			}
			foreach (string file in Directory.GetFiles(dir))
			{
				if (SCommon.EqualsIgnoreCase(Path.GetFileName(file), Consts.GIT_ATTRIBUTES_LOCAL_FILE))
				{
					// Skip
				}
				else
				{
					dest.Add(file);
				}
			}
		}

		private void CollectDeleteDirectoryEntries(string dir, List<string> dest)
		{
			foreach (string subDir in Directory.GetDirectories(dir))
			{
				this.CollectDeleteDirectoryEntries(subDir, dest);
			}
			foreach (string file in Directory.GetFiles(dir))
			{
				dest.Add(file);
			}
			dest.Add(dir);
		}

		private void CopyToRepository(string rDir, string wDir)
		{
			foreach (string path in this.GetCopyDirectoryEntries(rDir).ProgressBar("Copy"))
			{
				string relPath = SCommon.ChangeRoot(path, rDir);

				string rPath = Path.Combine(rDir, relPath);
				string wPath = Path.Combine(wDir, relPath);

				if (Directory.Exists(rPath))
				{
					SCommon.CreateDir(wPath);
				}
				else if (File.Exists(rPath))
				{
					File.Copy(rPath, wPath);
				}
				else
				{
					throw new Exception("Bad path: " + path);
				}
			}
		}

		private List<string> GetCopyDirectoryEntries(string dir)
		{
			List<string> dest = new List<string>();
			this.CollectCopyDirectoryEntries(dir, dest);
			return dest;
		}

		private void CollectCopyDirectoryEntries(string dir, List<string> dest)
		{
			foreach (string subDir in Directory.GetDirectories(dir))
			{
				dest.Add(subDir);
				this.CollectCopyDirectoryEntries(subDir, dest);
			}
			foreach (string file in Directory.GetFiles(dir))
			{
				dest.Add(file);
			}
		}
	}
}
