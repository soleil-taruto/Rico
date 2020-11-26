using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;

namespace Charlotte.CSSolutions
{
	public class CSProjectFile
	{
		private string _file;

		public CSProjectFile(string file)
		{
			_file = file;
		}

		private class SCO_RangeInfo
		{
			public int Start;
			public int End;

			public IEnumerable<int> Sequence()
			{
				return Enumerable.Range(this.Start, this.End - this.Start);
			}
		}

		public void ShuffleCompileOrder()
		{
			string[] lines = File.ReadAllLines(_file, Encoding.UTF8);

			SCO_RangeInfo[] ranges = SCO_GetRanges(lines).ToArray();

			SCO_RangeInfo[] shuffledRanges = ranges.ToArray(); // .ToArray() as shallow copy
			SCommon.CRandom.Shuffle(shuffledRanges);

			File.WriteAllLines(_file, SCO_GetOutputLines(lines, ranges, shuffledRanges), Encoding.UTF8);
		}

		private static IEnumerable<SCO_RangeInfo> SCO_GetRanges(string[] lines)
		{
			for (int index = 0; index < lines.Length; index++)
			{
				if (lines[index].Trim().StartsWith("<Compile "))
				{
					if (lines[index].Trim().EndsWith("/>"))
					{
						yield return new SCO_RangeInfo()
						{
							Start = index,
							End = index + 1,
						};
					}
					else
					{
						int end;

						for (end = index + 1; ; end++)
							if (lines[end].Trim() == "</Compile>")
								break;

						yield return new SCO_RangeInfo()
						{
							Start = index,
							End = end + 1,
						};

						index = end;
					}
				}
			}
		}

		private static IEnumerable<string> SCO_GetOutputLines(string[] lines, SCO_RangeInfo[] ranges, SCO_RangeInfo[] shuffledRanges)
		{
			int lineIndex = 0;

			for (int rangeIndex = 0; rangeIndex < ranges.Length; rangeIndex++)
			{
				SCO_RangeInfo range = ranges[rangeIndex];

				while (lineIndex < range.Start)
					yield return lines[lineIndex++];

				foreach (string line in shuffledRanges[rangeIndex].Sequence().Select(v => lines[v]))
					yield return line;

				lineIndex = range.End;
			}
			while (lineIndex < lines.Length)
				yield return lines[lineIndex++];
		}
	}
}
