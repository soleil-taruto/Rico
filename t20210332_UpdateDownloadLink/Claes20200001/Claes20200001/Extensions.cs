using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
{
	public static class Extensions
	{
		// sync > @ ForEach_Sort

		public static IEnumerable<T> ForEach<T>(this IEnumerable<T> src, Action<T> action)
		{
			foreach (T element in src)
				action(element);

			return src;
		}

		public static IEnumerable<T> Sort<T>(this IEnumerable<T> src, Comparison<T> comp)
		{
			if (src is T[])
			{
				Array.Sort((T[])src, comp);
			}
			else if (src is List<T>)
			{
				((List<T>)src).Sort(comp);
			}
			else
			{
				List<T> list = src.ToList();
				list.Sort(comp);
				src = list;
			}
			return src;
		}

		// < sync
	}
}
