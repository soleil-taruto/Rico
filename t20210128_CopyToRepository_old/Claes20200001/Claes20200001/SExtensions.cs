using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
{
	public static class SExtensions
	{
		public static IEnumerable<T> ProgressBar<T>(this List<T> list, string prompt = null)
		{
			return Common.Foreach_ProgressBar(list, prompt);
		}
	}
}
