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
			public string File;
			public string[] Lines;
		}

		public enum 識別子タイプ_e
		{
			名前空間 = 1,
			関数,
			変数,
		}

		public class TagInfo
		{
			public string File;
			public int LineNo;
			public string 識別子名;
			public 識別子タイプ_e 識別子タイプ;
		}

		public List<string> SourceLines = new List<string>();
		public List<HtmlFileInfo> HtmlFiles = new List<HtmlFileInfo>();
		public List<TagInfo> Tags = new List<TagInfo>();
	}
}
