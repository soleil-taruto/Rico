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

			//Common.Pause();
		}

		private void Main4()
		{
			if (!Directory.Exists(Consts.SPEC_ROOT_DIR))
				throw new Exception("no SPEC_ROOT_DIR");

			if (!Directory.Exists(Consts.DOC_ROOT_DIR))
				throw new Exception("no DOC_ROOT_DIR");

			foreach (string htmlFile in Directory.GetFiles(Consts.SPEC_ROOT_DIR, "*.html", SearchOption.AllDirectories).Sort(SCommon.Comp))
			{
				string html = File.ReadAllText(htmlFile, Consts.HTML_ENCODING);
				int startIndex = 0;

				for (; ; )
				{
					Common.Enclosed enclosed = Common.GetEnclosed(html, Consts.LINK_START, Consts.LINK_END, startIndex);

					if (enclosed == null)
						break;

					ProcMain.WriteLog("enclosed.Inner: " + enclosed.Inner);

					string path = enclosed.Inner;
					string[] pathTokens = path.Split('/');

					//if (pathTokens.Length != 3)
					if (pathTokens.Length < 2)
						throw new Exception("Bad pathTokens");

					foreach (string pathToken in pathTokens)
						if (!Regex.IsMatch(pathToken, "^[._0-9A-Za-z]+$"))
							throw new Exception("Bad pathToken");

					if (!SCommon.EndsWithIgnoreCase(pathTokens[pathTokens.Length - 1], ".zip"))
						throw new Exception("Bad pathToken");

					string relDir = Path.Combine(pathTokens.Take(pathTokens.Length - 1).ToArray());
					string dir = Path.Combine(Consts.DOC_ROOT_DIR, relDir);

					if (Directory.Exists(dir))
					{
						string[] files = Directory.GetFiles(dir, "*.zip");

						Array.Sort(files, SCommon.CompIgnoreCase);

						string file = files[files.Length - 1];
						string relFile = SCommon.ChangeRoot(file, Consts.DOC_ROOT_DIR);
						string pathNew = relFile.Replace('\\', '/');

						ProcMain.WriteLog("pathNew: " + pathNew);

						html = enclosed.Left + pathNew + enclosed.Right;
						startIndex = enclosed.Left.Length + pathNew.Length + enclosed.CloseTag.Length;
					}
					else
					{
						startIndex = enclosed.Left.Length + enclosed.Inner.Length + enclosed.CloseTag.Length;
					}
				}
				File.WriteAllText(htmlFile, html, Consts.HTML_ENCODING);
			}
		}
	}
}
