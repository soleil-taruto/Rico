﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;
using Charlotte.CSSolutions;

namespace Charlotte
{
	public static class ElsaConfuser
	{
		public static void Perform(string solutionFile, string workDir)
		{
			solutionFile = SCommon.MakeFullPath(solutionFile);
			workDir = SCommon.MakeFullPath(workDir);

			if (!File.Exists(solutionFile))
				throw new Exception("no solutionFile");

			if (!Directory.Exists(workDir))
				throw new Exception("no workDir");

			string solutionDir = Path.GetDirectoryName(solutionFile);
			string workSolutionDir = Path.Combine(workDir, "tmpsol");
			string workSolutionFile = Path.Combine(workSolutionDir, Path.GetFileName(solutionFile));

			SCommon.DeletePath(workSolutionDir);
			SCommon.CopyDir(solutionDir, workSolutionDir);

			// まかり間違っても masterSol.Confuse() を実行しないように！

			CSSolution sol = new CSSolution(workSolutionFile);

			sol.Confuse();
			sol.Rebuild();

			CSSolution masterSol = new CSSolution(solutionFile);

			SCommon.DeletePath(masterSol.GetBinDir());

			if (File.Exists(sol.GetOutputExeFile())) // ? ビルド成功
			{
				SCommon.CopyDir(sol.GetBinDir(), masterSol.GetBinDir());
			}
			else // ? ビルド失敗
			{
				SCommon.CreateDir(masterSol.GetBinDir());

				Console.WriteLine("★★★ 警告 ★★★");
				Console.WriteLine("ビルドは失敗しました。");
				Console.WriteLine("エンターキーを押すと続行します。");
				Console.WriteLine("★★★");
				Console.ReadLine();
			}
		}
	}
}