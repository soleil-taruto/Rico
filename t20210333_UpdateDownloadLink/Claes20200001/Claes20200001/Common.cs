using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using Charlotte.Commons;

namespace Charlotte
{
	public static class Common
	{
		public static void Pause()
		{
			Console.WriteLine("Press ENTER key.");
			Console.ReadLine();
		}

		// sync > @ GetOutputDir

		#region GetOutputDir

		private static string GOD_Dir;

		public static string GetOutputDir()
		{
			if (GOD_Dir == null)
				GOD_Dir = GetOutputDir_Main();

			return GOD_Dir;
		}

		private static string GetOutputDir_Main()
		{
			for (int c = 1; c <= 999; c++)
			{
				string dir = "C:\\" + c;

				if (
					!Directory.Exists(dir) &&
					!File.Exists(dir)
					)
				{
					SCommon.CreateDir(dir);
					//SCommon.Batch(new string[] { "START " + dir });
					return dir;
				}
			}
			throw new Exception("C:\\1 ～ 999 は使用できません。");
		}

		public static void OpenOutputDir()
		{
			SCommon.Batch(new string[] { "START " + GetOutputDir() });
		}

		public static void OpenOutputDirIfCreated()
		{
			if (GOD_Dir != null)
			{
				OpenOutputDir();
			}
		}

		private static int NOP_Count = 0;

		public static string NextOutputPath()
		{
			return Path.Combine(GetOutputDir(), (++NOP_Count).ToString("D4"));
		}

		#endregion

		// < sync

		public class Enclosed
		{
			public string BeforeOpenTag;
			public string OpenTag;
			public string Left { get { return this.BeforeOpenTag + this.OpenTag; } }
			public string Inner;
			public string Right { get { return this.CloseTag + this.AfterCloseTag; } }
			public string CloseTag;
			public string AfterCloseTag;
		}

		public static Enclosed GetEnclosed(string str, string openTag, string closeTag, int startIndex = 0)
		{
			int p = str.IndexOf(openTag, startIndex);

			if (p == -1)
				return null;

			int q = str.IndexOf(closeTag, p + openTag.Length);

			if (q == -1)
				return null;

			int p1 = 0;
			int p2 = p;
			int p3 = p + openTag.Length;
			int p4 = q;
			int p5 = q + closeTag.Length;
			int p6 = str.Length;

			return new Enclosed()
			{
				BeforeOpenTag = str.Substring(p1, p2 - p1),
				OpenTag = str.Substring(p2, p3 - p2),
				Inner = str.Substring(p3, p4 - p3),
				CloseTag = str.Substring(p4, p5 - p4),
				AfterCloseTag = str.Substring(p5, p6 - p5),
			};
		}
	}
}
