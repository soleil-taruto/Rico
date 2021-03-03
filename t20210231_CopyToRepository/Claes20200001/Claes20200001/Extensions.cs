using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
{
	public static class Extensions
	{
		public static IEnumerable<T> Sort<T>(this IEnumerable<T> src, Comparison<T> comp)
		{
			List<T> list = src.ToList();
			list.Sort(comp);
			return list;
		}
	}
}
