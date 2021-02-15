using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
{
	public static class Extensions
	{
		public static IEnumerable<T> ProgressBar<T>(this T[] arr, string prompt = null)
		{
			return Common.ForEach_ProgressBar(arr, prompt);
		}
	}
}
