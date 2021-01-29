using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
{
	public static class Common
	{
		public static IEnumerable<T> ForEach_ProgressBar<T>(T[] arr, string prompt)
		{
			Console.WriteLine(prompt);
			Console.Write("[*");

			const int STAR_MAX = 76;
			int star = 0;

			for (int index = 0; index < arr.Length; index++)
			{
				int targStar = (index * STAR_MAX) / arr.Length;

				while (star < targStar)
				{
					Console.Write("*");
					star++;
				}
				yield return arr[index];
			}
			while (star < STAR_MAX)
			{
				Console.Write("*");
				star++;
			}
			Console.WriteLine("]");
		}
	}
}
