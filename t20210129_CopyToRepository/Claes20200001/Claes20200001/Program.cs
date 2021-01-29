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
				else
				{
					SCommon.DeletePath(file);
				}
			}
		}

		public void CopyToRepository(string rDir, string wDir)
		{
			Predicate<string> approveFile = file =>
			{
				return
					!SCommon.EqualsIgnoreCase(Path.GetExtension(file), ".exe") &&
					!SCommon.EqualsIgnoreCase(Path.GetExtension(file), ".obj");
			};

			foreach (string dir in Directory.GetDirectories(rDir))
			{
				SCommon.CopyDir(dir, Path.Combine(wDir, Path.GetFileName(dir)), approveFile);
			}
			foreach (string file in Directory.GetFiles(rDir))
			{
				if (approveFile(file))
				{
					File.Copy(file, Path.Combine(wDir, Path.GetFileName(file)));
				}
			}
		}
	}
}
