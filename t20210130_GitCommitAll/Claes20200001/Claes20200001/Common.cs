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
		private static string _gitExeFile = null;

		public static string GetGitExeFile()
		{
			if (_gitExeFile == null)
				_gitExeFile = GetGitExeFile_Main();

			return _gitExeFile;
		}

		private static string GetGitExeFile_Main()
		{
			const string PATH_00_ENV = "USERPROFILE";
			const string PATH_01 = @"AppData\Local\GitHubDesktop";
			const string PATH_02_PREFIX = "app-";
			const string PATH_03 = @"resources\app\git\cmd\git.exe";

			string dir_00 = Environment.GetEnvironmentVariable(PATH_00_ENV);

			if (string.IsNullOrEmpty(dir_00))
				throw new Exception("Bad dir_00");

			if (!Directory.Exists(dir_00))
				throw new Exception("no dir_00");

			string dir_01 = Path.Combine(dir_00, PATH_01);

			if (!Directory.Exists(dir_01))
				throw new Exception("no dir_01");

			string[] dir_02s = Directory.GetDirectories(dir_01);

			// HACK: バージョンを構成する番号の桁数が増えた場合を考慮していない。
			Array.Sort(dir_02s, (a, b) => SCommon.CompIgnoreCase(b, a)); // 降順

			foreach (string dir_02 in dir_02s)
			{
				if (SCommon.StartsWithIgnoreCase(Path.GetFileName(dir_02), PATH_02_PREFIX))
				{
					string file_03 = Path.Combine(dir_02, PATH_03);

					if (File.Exists(file_03))
					{
						file_03 = SCommon.MakeFullPath(file_03); // 2bs
						Console.WriteLine("GGEF_M_file_03: " + file_03); // cout
						return file_03;
					}
				}
			}
			throw new Exception("no file_03");
		}

		public static IEnumerable<string> E_GetRepoFiles(string repoDir)
		{
			foreach (string dir in Directory.GetDirectories(repoDir))
			{
				if (SCommon.EqualsIgnoreCase(Path.GetFileName(dir), ".git"))
					continue;

				foreach (string file in Directory.GetFiles(dir, "*", SearchOption.AllDirectories))
					yield return file;
			}
			foreach (string file in Directory.GetFiles(repoDir))
			{
				if (SCommon.EqualsIgnoreCase(Path.GetFileName(file), ".gitattributes"))
					continue;

				yield return file;
			}
		}

		public static void Distinct<T>(List<T> list, Comparison<T> comp)
		{
			for (int index = list.Count - 1; 1 <= index; index--)
				if (list.Take(index).Any(element => comp(element, list[index]) == 0))
					list.RemoveAt(index);
		}

		/// <summary>
		/// 始点から終点までの間の指定レートの位置の値を返す。
		/// </summary>
		/// <param name="a">始点</param>
		/// <param name="b">終点</param>
		/// <param name="rate">レート</param>
		/// <returns>レートの値</returns>
		public static double AToBRate(double a, double b, double rate)
		{
			return a + (b - a) * rate;
		}

		/// <summary>
		/// 始点から終点までの間の位置をレートに変換する。
		/// </summary>
		/// <param name="a">始点</param>
		/// <param name="b">終点</param>
		/// <param name="value">位置</param>
		/// <returns>レート</returns>
		public static double RateAToB(double a, double b, double value)
		{
			return (value - a) / (b - a);
		}
	}
}
