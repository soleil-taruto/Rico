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

			ReadSourceFiles();
			CheckSyntax();
			WriteTagsFile();
			WriteHtmlFiles();
		}

		private void ReadSourceFiles()
		{
			foreach (string file in Directory.GetFiles(Ground.I.SourceDir, "*.js", SearchOption.AllDirectories))
			{
				string nameOfSpace = Path.GetFileNameWithoutExtension(file);
				string[] lines = File.ReadAllLines(file, SCommon.ENCODING_SJIS);

				Ground.I.Tags.Add(new Ground.TagInfo(file, 1, nameOfSpace, Ground.識別子タイプ_e.名前空間));

				for (int index = 0; index < lines.Length; index++)
				{
					string line = lines[index];

					line = line.Replace("@@", nameOfSpace);

					Ground.I.SourceLines.Add(line);

					string tLine = line;
					tLine = tLine.Replace('\t', ' ');
					tLine = tLine.Replace('　', ' ');
					for (int c = 0; c < 20; c++) tLine = tLine.Replace("  ", " ");
					tLine = tLine.Trim();

					if (Regex.IsMatch(tLine, "^var [^ ]+ ?;.*$")) // ? 広域変数宣言
					{
						string varName = tLine.Substring(4).Split(';')[0].Trim();

						Ground.I.Tags.Add(new Ground.TagInfo(file, index + 1, varName, Ground.識別子タイプ_e.変数));
					}
					if (Regex.IsMatch(tLine, "^function [^ ]+ ?\\(.*$")) // ? 広域関数宣言
					{
						string funcName = tLine.Substring(9).Split('(')[0].Trim();

						Ground.I.Tags.Add(new Ground.TagInfo(file, index + 1, funcName, Ground.識別子タイプ_e.関数));
					}
				}
			}
			foreach (string file in Directory.GetFiles(Ground.I.SourceDir, "*.html", SearchOption.AllDirectories))
			{
				string text = File.ReadAllText(file, Encoding.UTF8);

				Ground.I.HtmlFiles.Add(new Ground.HtmlFileInfo(file, text));
			}
		}

		private void CheckSyntax()
		{
			throw new NotImplementedException();
		}

		private void WriteTagsFile()
		{
			throw new NotImplementedException();
		}

		private void WriteHtmlFiles()
		{
			throw new NotImplementedException();
		}
	}
}
