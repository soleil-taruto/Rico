using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
{
	public class Ground
	{
		public static Ground I = new Ground();

		private Ground()
		{ }

		public string ProjectDir;
		public string SourceDir;
		public string DataDir;
		public string OutputDir;
		public string TagsFile;

		public class HtmlFileInfo
		{
			public string FilePath;
			public string Text;

			public HtmlFileInfo(string file, string text)
			{
				this.FilePath = file;
				this.Text = text;
			}
		}

		public enum 識別子タイプ_e
		{
			名前空間 = 1,
			変数,
			関数,
		}

		public class TagInfo
		{
			public string FilePath;
			public int LineNo;
			public string 識別子名;
			public 識別子タイプ_e 識別子タイプ;

			public TagInfo(string file, int lineNo, string 識別子名, 識別子タイプ_e 識別子タイプ)
			{
				this.FilePath = file;
				this.LineNo = lineNo;
				this.識別子名 = 識別子名;
				this.識別子タイプ = 識別子タイプ;
			}
		}

		public List<string> SourceLines = new List<string>();
		public List<TagInfo> Tags = new List<TagInfo>();
		public List<HtmlFileInfo> HtmlFiles = new List<HtmlFileInfo>();
	}
}
