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
				Main4(ar);
			}
			Common.OpenOutputDirIfCreated();
		}

		private void Main3()
		{
			// -- choose one --

			Main4(ProcMain.ArgsReader);
			//new Test0001().Test01();
			//new Test0002().Test01();
			//new Test0003().Test01();

			// --

			//Common.Pause();
		}

		private void Main4(ArgsReader ar)
		{
			Ground.I.ProjectDir = ar.NextArg();
			Ground.I.ProjectDir = SCommon.MakeFullPath(Ground.I.ProjectDir);
			Ground.I.SourceDir = Path.Combine(Ground.I.ProjectDir, "src");
			Ground.I.DataDir = Path.Combine(Ground.I.ProjectDir, "dat");
			Ground.I.OutputDir = Path.Combine(Ground.I.ProjectDir, "out");
			Ground.I.TagsFile = Path.Combine(Ground.I.ProjectDir, "tags");

			if (!Directory.Exists(Ground.I.ProjectDir))
				throw new Exception("no ProjectDir: " + Ground.I.ProjectDir);

			if (!Directory.Exists(Ground.I.SourceDir))
				throw new Exception("no SourceDir: " + Ground.I.SourceDir);

			if (!Directory.Exists(Ground.I.DataDir))
				throw new Exception("no DataDir: " + Ground.I.DataDir);

			SCommon.DeletePath(Ground.I.OutputDir);
			SCommon.CreateDir(Ground.I.OutputDir);

			SCommon.DeletePath(Ground.I.TagsFile);

			ReadAllSourceFile();

		}
	}
}
