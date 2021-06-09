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

		public class PairInfo<T>
		{
			public T A;
			public T B;
		}

		public static IEnumerable<PairInfo<T>> Merge<T>(IList<T> list1, IList<T> list2, Comparison<T> comp)
		{
			T[] arr1 = list1.ToArray(); // shallow copy
			T[] arr2 = list2.ToArray(); // shallow copy

			Array.Sort(arr1, comp);
			Array.Sort(arr2, comp);

			int index1 = 0;
			int index2 = 0;

			while (index1 < arr1.Length && index2 < arr2.Length)
			{
				T a = arr1[index1];
				T b = arr2[index2];

				int ret = comp(a, b);

				if (ret < 0) // ? a < b
				{
					yield return new PairInfo<T>()
					{
						A = a,
						B = default(T),
					};

					index1++;
				}
				else if (0 < ret) // ? a > b
				{
					yield return new PairInfo<T>()
					{
						A = default(T),
						B = b,
					};

					index2++;
				}
				else // ? a == b
				{
					yield return new PairInfo<T>()
					{
						A = a,
						B = b,
					};

					index1++;
					index2++;
				}
			}
			for (; index1 < arr1.Length; index1++)
			{
				yield return new PairInfo<T>()
				{
					A = arr1[index1],
					B = default(T),
				};
			}
			for (; index2 < arr2.Length; index2++)
			{
				yield return new PairInfo<T>()
				{
					A = default(T),
					B = arr2[index2],
				};
			}
		}
	}
}
