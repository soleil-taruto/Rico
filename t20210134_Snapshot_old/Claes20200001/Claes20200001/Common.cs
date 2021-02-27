using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;

namespace Charlotte
{
	public static class Common
	{
		public static IEnumerable<string> GetAllPaths(string rootDir)
		{
			foreach (string dir in Directory.GetDirectories(rootDir, "*", SearchOption.AllDirectories))
				yield return dir;

			foreach (string file in Directory.GetFiles(rootDir, "*", SearchOption.AllDirectories))
				yield return file;
		}
	}
}
