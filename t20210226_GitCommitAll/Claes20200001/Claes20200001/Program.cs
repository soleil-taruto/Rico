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
				TestMain(); // テスト
			}
			else
			{
				ProductMain(ar); // 本番
			}
		}

		private void TestMain()
		{
			// -- choose one --

			new Test0001().Test01();
			//new Test0001().Test02();
			//new Test0001().Test03();

			// --

			Console.WriteLine("Press ENTER to exit.");
			Console.ReadLine();
		}

		private void ProductMain(ArgsReader ar)
		{
			this.CommitAll(ar.NextArg());
		}

		public void CommitAll(string outBatFile)
		{
			outBatFile = SCommon.MakeFullPath(outBatFile);

			if (!Directory.Exists(Consts.REPOSITORIES_ROOT_DIR))
				throw new Exception("no REPOSITORIES_ROOT_DIR");

			string[] repoDirs = Directory.GetDirectories(Consts.REPOSITORIES_ROOT_DIR);

			foreach (string repoDir in repoDirs)
				if (!this.IsRepository(repoDir))
					throw new Exception("not repository: " + repoDir); // リポジトリ以外のディレクトリは無いはず！

			List<string> outBatLines = new List<string>();

			foreach (string repoDir in repoDirs)
				this.Commit(repoDir, outBatLines);

			File.WriteAllLines(outBatFile, outBatLines, SCommon.ENCODING_SJIS);
		}

		private bool IsRepository(string dir)
		{
			string gitDir = Path.Combine(dir, Consts.GIT_LOCAL_DIR);
			string gitAttributesFile = Path.Combine(dir, Consts.GIT_ATTRIBUTES_LOCAL_FILE);

			return
				Directory.Exists(gitDir) &&
				File.Exists(gitAttributesFile);
		}

		private void Commit(string repoDir, List<string> dest)
		{
			dest.Add("CD /D C:\\temp"); // 2bs
			dest.Add("CD /D \"" + repoDir + "\"");
			dest.Add(Common.GetGitExeFile() + " add *");
			dest.Add(Common.GetGitExeFile() + " commit -m \"Backup " + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"");
			//dest.Add(Common.GetGitExeFile() + " push");
			dest.Add("CD /D C:\\temp"); // 2bs
		}
	}
}
