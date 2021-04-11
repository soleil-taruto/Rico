using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
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
				Main3();
			}
			else
			{
				Main4();
			}
			Common.OpenOutputDirIfCreated();
		}

		private void Main3()
		{
			// -- choose one --

			Main4();
			//new Test0001().Test01();
			//new Test0002().Test01();
			//new Test0003().Test01();

			// --

			Common.Pause();
		}

		private void Main4()
		{
			if (!Directory.Exists(Consts.SPEC_ROOT_DIR))
				throw new Exception("no SPEC_ROOT_DIR");

			if (!Directory.Exists(Consts.DOC_ROOT_DIR))
				throw new Exception("no DOC_ROOT_DIR");

			if (!Directory.Exists(Consts.DEVENV_ROOT_DIR))
				throw new Exception("no DEVENV_ROOT_DIR");

			foreach (string htmlFile in Directory.GetFiles(Consts.SPEC_ROOT_DIR, "*.html", SearchOption.AllDirectories).Sort(SCommon.Comp))
			{
				Console.WriteLine("* " + htmlFile); // cout

				string html = File.ReadAllText(htmlFile, Consts.HTML_ENCODING);
				int startIndex = 0;

				for (; ; )
				{
					Common.Enclosed enclosed = Common.GetEnclosed(html, Consts.LINK_START, Consts.LINK_END, startIndex);

					if (enclosed == null)
						break;

					string path = enclosed.Inner;

					if (
						Regex.IsMatch(path, "^[/._0-9A-Za-z]+$") &&
						SCommon.EndsWithIgnoreCase(path, ".zip")
						)
					{
						Console.WriteLine("P.< " + path); // cout

						string[] pathTokens = path.Split('/');

						// チェック pathTokens
						{
							if (pathTokens.Length < 2)
								throw new Exception("Bad pathTokens (length < 2)");

							foreach (string pathToken in pathTokens)
								if (!Regex.IsMatch(pathToken, "^[._0-9A-Za-z]+$"))
									throw new Exception("Bad pathToken (not allow characters)");

							if (!SCommon.EndsWithIgnoreCase(pathTokens[pathTokens.Length - 1], ".zip"))
								throw new Exception("Bad pathToken (not *.zip)");

							if (!Regex.IsMatch(pathTokens[pathTokens.Length - 2], "^[A-Za-z][0-9]{8}_[_0-9A-Za-z]+$"))
								throw new Exception("Bad pathToken (not x99999999_*)");
						}

						ChangeToNewestVersionPathTokensIfNeeded(pathTokens);

						string relDir = Path.Combine(pathTokens.Take(pathTokens.Length - 1).ToArray());
						string dir = Path.Combine(Consts.DOC_ROOT_DIR, relDir);

						if (Directory.Exists(dir))
						{
							string[] files = Directory.GetFiles(dir, "*.zip");

							Array.Sort(files, SCommon.CompIgnoreCase);

							string file = files[files.Length - 1];
							string relFile = SCommon.ChangeRoot(file, Consts.DOC_ROOT_DIR);
							string pathNew = relFile.Replace('\\', '/');

							Console.WriteLine("P.> " + pathNew); // cout

							html = enclosed.Left + pathNew + enclosed.Right;
						}
					}
					startIndex = html.Length - enclosed.AfterCloseTag.Length;
				}
				File.WriteAllText(htmlFile, html, Consts.HTML_ENCODING);
			}
		}

		private void ChangeToNewestVersionPathTokensIfNeeded(string[] pathTokens)
		{
			string projectParentDir = Consts.DEVENV_ROOT_DIR;

			foreach (string pathToken in pathTokens.Take(pathTokens.Length - 2))
				projectParentDir = Path.Combine(projectParentDir, pathToken);

			string currProjectLocalDir = pathTokens[pathTokens.Length - 2];
			string currProjectDir = Path.Combine(projectParentDir, currProjectLocalDir);

			if (!Directory.Exists(projectParentDir))
			{
				Console.WriteLine("★プロジェクトの親ディレクトリ喪失 --> とても古いプロジェクト");
				return;
			}
			if (!Directory.Exists(currProjectDir))
			{
				Console.WriteLine("★プロジェクトのディレクトリ喪失 --> 古いプロジェクト");
				return;
			}

			string[] projectDirs = Directory.GetDirectories(projectParentDir)
				.Where(dir =>
				{
					string localDir = Path.GetFileName(dir);

					return
						Regex.IsMatch(localDir, "^[A-Za-z][0-9]{8}_[_0-9A-Za-z]+$") &&
						SCommon.EqualsIgnoreCase(localDir.Substring(10), currProjectLocalDir.Substring(10)) &&
						File.Exists(Path.Combine(dir, Consts.NEWEST_VERSION_SYMBOL_FILE_LOCAL_NAME));
				})
				.ToArray();

			if (projectDirs.Length == 0)
			{
				Console.WriteLine("★アクティブなプロジェクト無し --> 非アクティブなプロジェクト");
			}
			else if (2 <= projectDirs.Length)
			{
				Console.WriteLine("★複数のアクティブなプロジェクト --> アクティブすぎて分からない");
			}
			else
			{
				string projectLocalDir = Path.GetFileName(projectDirs[0]);

				if (!SCommon.EqualsIgnoreCase(projectLocalDir, currProjectLocalDir))
				{
					Console.WriteLine("D.< " + currProjectLocalDir); // cout
					Console.WriteLine("D.> " + projectLocalDir); // cout

					pathTokens[pathTokens.Length - 2] = projectLocalDir;
				}
			}
		}
	}
}
