using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Charlotte.Commons;

namespace Charlotte
{
	public static class Common
	{
		public static string LZPad(string str, int minlen = 1, string padding = "0")
		{
			while (str.Length < minlen)
			{
				str = padding + str;
			}
			return str;
		}

		public static string GetHash(string file)
		{
			file = SCommon.MakeFullPath(file);

			using (WorkingDir wd = new WorkingDir())
			{
				SCommon.Batch(new string[] { string.Format(@"C:\Factory\Petra\SimpleMD5.exe ""{0}"" > out.txt", file) }, wd.GetPath("."));

				string hash = File.ReadAllText(wd.GetPath("out.txt"), Encoding.ASCII);
				hash = hash.Trim();
				hash = hash.ToLower();

				if (!Regex.IsMatch(hash, "^[0-9a-f]{32}$"))
					throw new Exception("Bad hash");

				return hash;
			}
		}
	}
}
