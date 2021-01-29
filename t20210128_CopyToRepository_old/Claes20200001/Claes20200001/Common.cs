using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
{
	public static class Common
	{
		public static IEnumerable<T> Foreach_ProgressBar<T>(List<T> list, string prompt)
		{
			Console.WriteLine(prompt);
			Console.Write("[*");

			const int STAR_MAX = 76;
			int star = 0;

			for (int index = 0; index < list.Count; index++)
			{
				int targStar = (index * STAR_MAX) / list.Count;

				while (star < targStar)
				{
					Console.Write("*");
					star++;
				}
				yield return list[index];
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
