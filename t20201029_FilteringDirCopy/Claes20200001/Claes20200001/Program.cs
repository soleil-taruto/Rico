using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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
				// -- choose one --

				new Test0001().Test01();
				//new Test0001().Test02();
				//new Test0001().Test03();

				// --
			}
			else
			{
				this.Main3(ar);
			}
		}

		private void Main3(ArgsReader ar)
		{
			List<Predicate<string>> filters = new List<Predicate<string>>();

			for (; ; )
			{
				if (ar.ArgIs("/P+"))
				{
					string acceptPattern = ar.NextArg();

					Console.WriteLine("acceptPattern: [" + acceptPattern + "]");

					filters.Add(v => SCommon.ContainsIgnoreCase(v, acceptPattern));
					continue;
				}
				if (ar.ArgIs("/P-"))
				{
					string rejectPattern = ar.NextArg();

					Console.WriteLine("rejectPattern: [" + rejectPattern + "]");

					filters.Add(v => !SCommon.ContainsIgnoreCase(v, rejectPattern));
					continue;
				}
				break;
			}
			string rDir = ar.NextArg();
			string wDir = ar.NextArg();

			Predicate<string> filter = v => !filters.Any(w => !w(v));

			if (string.IsNullOrWhiteSpace(rDir))
				throw new Exception("rDir is empty");

			if (string.IsNullOrWhiteSpace(wDir))
				throw new Exception("wDir is empty");

			if (!Directory.Exists(rDir))
				throw new Exception("no rDir: " + rDir);

			FilteringDirCopy(rDir, wDir, filter);
		}

		private void FilteringDirCopy(string rDir, string wDir, Predicate<string> filter)
		{
			SCommon.CreateDir(wDir);

			foreach (string dir in Directory.GetDirectories(rDir))
				if (Filtering(dir, filter))
					FilteringDirCopy(dir, Path.Combine(wDir, Path.GetFileName(dir)), filter);

			foreach (string file in Directory.GetFiles(rDir))
				if (Filtering(file, filter))
					File.Copy(file, Path.Combine(wDir, Path.GetFileName(file)));
		}

		private bool Filtering(string path, Predicate<string> filter)
		{
			path = SCommon.ToFullPath(path);
			path += "*";

			return filter(path);
		}
	}
}
