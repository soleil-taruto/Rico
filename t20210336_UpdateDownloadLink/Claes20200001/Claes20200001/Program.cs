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

			if (!Directory.Exists(Consts.DOWNLOAD_ROOT_DIR))
				throw new Exception("no DOWNLOAD_ROOT_DIR");

			foreach (string dataJSFile in Directory.GetFiles(Consts.SPEC_ROOT_DIR, Consts.DATA_JS_LOCAL_NAME, SearchOption.AllDirectories).OrderBy(SCommon.Comp))
			{
				Console.WriteLine("* " + dataJSFile); // cout

				string[] lines = File.ReadAllLines(dataJSFile, Consts.DATA_JS_ENCODING);

				string signature = lines[1];
				string relDir = lines[2];
				string wildCard = lines[3];

				if (signature != Consts.DATA_JS_SIGNATURE)
					continue;

				if (string.IsNullOrEmpty(relDir))
					throw new Exception("Bad relDir");

				if (string.IsNullOrEmpty(wildCard))
					throw new Exception("Bad wildCard");

				string downloadParentDir = Path.Combine(Consts.DOWNLOAD_ROOT_DIR, relDir);

				if (!Directory.Exists(downloadParentDir))
					throw new Exception("no downloadParentDir");

				string[] downloadDirs = Directory.GetDirectories(downloadParentDir, wildCard).ToArray();

				if (downloadDirs.Length < 1)
					throw new Exception("no downloadDirs");

				string downloadDir = downloadDirs
					.OrderBy(SCommon.CompIgnoreCase)
					.Last(dir => true);

				string downloadFile = Directory.GetFiles(downloadDir)
					.Where(file => SCommon.EndsWithIgnoreCase(file, Consts.DOWNLOAD_FILE_SUFFIX))
					.OrderBy(SCommon.CompIgnoreCase)
					.Last(file => true);

				string downloadRelFile = SCommon.ChangeRoot(downloadFile, Consts.DOWNLOAD_ROOT_DIR);
				string downloadUrlRelPath = downloadRelFile.Replace('\\', '/');
				string downloadUrl = Consts.DOWNLOAD_URL_PREFIX + downloadUrlRelPath;

				lines = new string[]
				{
					"/*",
					signature,
					relDir,
					wildCard,
					"*/",
					"let ccsp_download_link = \"" + downloadUrl + "\";",
				};

				using (WorkingDir wd = new WorkingDir())
				{
					string testOutFile = wd.MakePath();

					File.WriteAllLines(testOutFile, lines, Consts.DATA_JS_ENCODING);

					if (SCommon.Comp(File.ReadAllBytes(testOutFile), File.ReadAllBytes(dataJSFile)) == 0) // ? 同じ内容
					{
						Console.WriteLine("同じ内容なので更新しませんでした。"); // cout
					}
					else
					{
						File.Copy(testOutFile, dataJSFile, true);

						Console.WriteLine("更新しました。"); // cout
					}
				}
			}
		}
	}
}
