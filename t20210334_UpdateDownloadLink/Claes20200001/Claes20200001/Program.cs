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

			foreach (string dataJSFile in Directory.GetFiles(Consts.SPEC_ROOT_DIR, Consts.DATA_JS_LOCAL_NAME, SearchOption.AllDirectories).Sort(SCommon.Comp))
			{
				Console.WriteLine("* " + dataJSFile); // cout

				string[] lines = File.ReadAllLines(dataJSFile, Consts.DATA_JS_ENCODING);

				string urlPrefix = lines[1];
				string downloadFilesDir = lines[2];

				if (string.IsNullOrEmpty(urlPrefix))
					throw new Exception("Bad urlPrefix");

				if (string.IsNullOrEmpty(downloadFilesDir) && !Directory.Exists(downloadFilesDir))
					throw new Exception("Bad downloadFilesDir");

				string downloadFile = Directory.GetFiles(downloadFilesDir)
					.Where(file => SCommon.EndsWithIgnoreCase(file, Consts.DOWNLOAD_FILE_SUFFIX))
					.Sort(SCommon.CompIgnoreCase)
					.Last(file => true);

				lines = new string[]
				{
					"/*",
					urlPrefix,
					downloadFilesDir,
					"*/",
					"let ccsp_download_link = \"" + urlPrefix + Path.GetFileName(downloadFile) + "\";",
				};

				File.WriteAllLines(dataJSFile, lines, Consts.DATA_JS_ENCODING);
			}
		}
	}
}
