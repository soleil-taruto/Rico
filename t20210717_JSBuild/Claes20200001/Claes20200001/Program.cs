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

			//Main4(ProcMain.ArgsReader);
			Main4(new ArgsReader(new string[] { @"C:\Dev\Gattonero\t20210717_Tests" }));
			//Main4(new ArgsReader(new string[] { "/E", @"C:\Dev\Gattonero\t20210717_Tests" }));
			//new Test0001().Test01();
			//new Test0002().Test01();
			//new Test0003().Test01();

			// --

			//Common.Pause();
		}

		private void Main4(ArgsReader ar)
		{
			bool embedFlag = false;

			do
			{
				if (ar.ArgIs("/E"))
				{
					embedFlag = true;
					continue;
				}
			}
			while (false); // 流れ落ちる。

			Ground.I.ProjectDir = ar.NextArg();
			Ground.I.ProjectDir = SCommon.MakeFullPath(Ground.I.ProjectDir);
			Ground.I.SourceDir = Path.Combine(Ground.I.ProjectDir, "src");
			Ground.I.OutputDir = Path.Combine(Ground.I.ProjectDir, "out");
			Ground.I.TagsFile = Path.Combine(Ground.I.SourceDir, "tags");

			if (!Directory.Exists(Ground.I.ProjectDir))
				throw new Exception("no ProjectDir: " + Ground.I.ProjectDir);

			if (!Directory.Exists(Ground.I.SourceDir))
				throw new Exception("no SourceDir: " + Ground.I.SourceDir);

			SCommon.DeletePath(Ground.I.OutputDir);
			SCommon.CreateDir(Ground.I.OutputDir);

			SCommon.DeletePath(Ground.I.TagsFile);

			ReadSourceFiles();
			SyntaxCheck();

			if (embedFlag)
				Embed();

			WriteTagsFile();
			WriteHtmlFiles();
		}

		private void ReadSourceFiles()
		{
			foreach (string file in Directory.GetFiles(Ground.I.SourceDir, "*.js", SearchOption.AllDirectories).OrderBy(Common.CompPath))
			{
				Ground.I.SourceLines.Add("");
				Ground.I.SourceLines.Add("//");
				Ground.I.SourceLines.Add("// " + file);
				Ground.I.SourceLines.Add("//");
				Ground.I.SourceLines.Add("");

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
					//tLine = tLine.Replace('　', ' '); // 全角はケアしない。
					for (int c = 0; c < 20; c++) tLine = tLine.Replace("  ", " ");
					tLine = tLine.Replace("*", ""); // function* --> function
					//tLine = tLine.Trim(); // トリムしない！

					if (Regex.IsMatch(tLine, "^var [^ ]+ ?[;=].*$")) // ? 広域変数宣言
					{
						string varName = tLine.Substring(4).Split(';')[0].Split('=')[0].Trim();

						Ground.I.Tags.Add(new Ground.TagInfo(file, index + 1, varName, Ground.識別子タイプ_e.変数));
					}
					if (Regex.IsMatch(tLine, "^function [^ ]+ ?\\(.*$")) // ? 広域関数宣言
					{
						string funcName = tLine.Substring(9).Split('(')[0].Trim();

						Ground.I.Tags.Add(new Ground.TagInfo(file, index + 1, funcName, Ground.識別子タイプ_e.関数));
					}
				}

				Ground.I.SourceLines.Add("");
				Ground.I.SourceLines.Add("//");
				Ground.I.SourceLines.Add("// " + file + " </>");
				Ground.I.SourceLines.Add("//");
				Ground.I.SourceLines.Add("");
			}
			foreach (string file in Directory.GetFiles(Ground.I.SourceDir, "*.html", SearchOption.AllDirectories).OrderBy(Common.CompPath))
			{
				string text = File.ReadAllText(file, Encoding.UTF8);

				Ground.I.HtmlFiles.Add(new Ground.HtmlFileInfo(file, text));
			}
		}

		private void SyntaxCheck()
		{
			Ground.I.Tags.Sort((a, b) =>
			{
				int ret = SCommon.Comp(a.識別子名, b.識別子名);

				if (ret != 0)
					return ret;

				ret = (int)a.識別子タイプ - (int)b.識別子タイプ;
				return ret;
			});

			for (int index = 1; index < Ground.I.Tags.Count; index++)
			{
				Ground.TagInfo tag1 = Ground.I.Tags[index - 1];
				Ground.TagInfo tag2 = Ground.I.Tags[index - 0];

				if (tag1.識別子名 == tag2.識別子名)
				{
					throw new Exception("同じ識別子があります。" + tag1.識別子名);
				}
			}

			Ground.I.HtmlFiles.Sort((a, b) =>
			{
				int ret = SCommon.CompIgnoreCase(Path.GetFileName(a.FilePath), Path.GetFileName(b.FilePath));

				if (ret != 0)
					return ret;

				ret = Common.CompPath(a.FilePath, b.FilePath);
				return ret;
			});

			for (int index = 1; index < Ground.I.HtmlFiles.Count; index++)
			{
				Ground.HtmlFileInfo hf1 = Ground.I.HtmlFiles[index - 1];
				Ground.HtmlFileInfo hf2 = Ground.I.HtmlFiles[index - 0];

				if (SCommon.EqualsIgnoreCase(Path.GetFileName(hf1.FilePath), Path.GetFileName(hf2.FilePath)))
				{
					throw new Exception("同じファイル名があります。" + Path.GetFileName(hf1.FilePath));
				}
			}
		}

		private void Embed()
		{
			for (int index = 0; index < Ground.I.SourceLines.Count; index++)
			{
				string line = Ground.I.SourceLines[index];
				string[] encl = Common.ParseEnclosed(line, "\"", "\"");

				if (encl != null)
				{
					string href = encl[2];

					if (Common.IsFairHrefRelPath(href))
					{
						string file = href;

						file = file.Replace('/', '\\');
						file = Common.MakeFullPathByBaseDir(file, Ground.I.OutputDir);

						if (File.Exists(file))
						{
							string ext = Path.GetExtension(file);
							string mediaType = Common.GetMediaTypeByExt(ext);
							byte[] data = File.ReadAllBytes(file);
							string b64Data = SCommon.Base64.I.Encode(data);

							encl[2] = "data:" + mediaType + ";base64," + b64Data;
						}
					}
					line = string.Join("", encl);
				}
				Ground.I.SourceLines[index] = line;
			}
		}

		private void WriteTagsFile()
		{
			File.WriteAllLines(Ground.I.TagsFile, E_GetTagLines(), SCommon.ENCODING_SJIS);
		}

		private IEnumerable<string> E_GetTagLines()
		{
			foreach (Ground.TagInfo tag in Ground.I.Tags)
			{
				yield return tag.FilePath + "(" + tag.LineNo + ") : " + tag.識別子名 + " // " + tag.識別子タイプ;
			}
		}

		private void WriteHtmlFiles()
		{
			foreach (Ground.HtmlFileInfo hf in Ground.I.HtmlFiles)
			{
				string file = Path.Combine(Ground.I.OutputDir, Path.GetFileName(hf.FilePath));
				string text = hf.Text;

				text = text.Replace("${Source}", SCommon.LinesToText(Ground.I.SourceLines));

				File.WriteAllText(file, text, Encoding.UTF8);
			}
		}
	}
}
