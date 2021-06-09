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

		public static string[] GetHashes(string[] files)
		{
			using (WorkingDir wd = new WorkingDir())
			{
				string fileListFile = wd.MakePath();
				string hashesFile = wd.MakePath();

				File.WriteAllLines(fileListFile, files, SCommon.ENCODING_SJIS);

				SCommon.Batch(new string[]
				{
					string.Format(@"C:\Factory\Petra\FileList2MD5List.exe ""{0}"" ""{1}""", fileListFile, hashesFile),
				});

				string[] hashes = File.ReadAllLines(hashesFile, Encoding.ASCII)
					.Select(hash => hash.ToLower()) // 2bs
					.ToArray();

				if (files.Length != hashes.Length)
					throw new Exception("Bad hashes");

				foreach (string hash in hashes)
					if (!Regex.IsMatch(hash, "^[0-9a-f]{32}$"))
						throw new Exception("Bad hash");

				return hashes;
			}
		}
	}
}
