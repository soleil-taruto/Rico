﻿using System;
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

			new Test0001().Test01();
			//new Test0001().Test02();
			//new Test0001().Test03();

			// --

			Console.WriteLine("Press ENTER to exit.");
			Console.ReadLine();
		}

		private void ProductMain()
		{
			this.CommitAll();
		}

		private void CommitAll()
		{
			if (!Directory.Exists(Consts.REPOSITORIES_ROOT_DIR))
				throw new Exception("no REPOSITORIES_ROOT_DIR");

			string[] repoDirs = Directory.GetDirectories(Consts.REPOSITORIES_ROOT_DIR);

			foreach (string repoDir in repoDirs)
				if (!this.IsRepository(repoDir))
					throw new Exception("not repository: " + repoDir); // リポジトリ以外のディレクトリは無いはず！

			foreach (string repoDir in repoDirs)
				this.Commit(repoDir);
		}

		private bool IsRepository(string dir)
		{
			string gitDir = Path.Combine(dir, Consts.GIT_LOCAL_DIR);
			string gitAttributesFile = Path.Combine(dir, Consts.GIT_ATTRIBUTES_LOCAL_FILE);

			return
				Directory.Exists(gitDir) &&
				File.Exists(gitAttributesFile);
		}

		private void Commit(string repoDir)
		{
			ProcMain.WriteLog("Commit.1 " + repoDir); // cout

			string[] lines = SCommon.Batch(
				new string[]
				{
					Common.GetGitExeFile() + " add *",
					Common.GetGitExeFile() + " commit -m \"Backup " + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"",
					//Common.GetGitExeFile() + " push",
				},
				repoDir,
				SCommon.StartProcessWindowStyle_e.MINIMIZED
				);

			ProcMain.WriteLog("Commit.2"); // cout

			foreach (string line in lines)
				Console.WriteLine(line);
		}
	}
}
