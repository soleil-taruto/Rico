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

		public void RenameCompiles()
		{
			RC_RenameCompiles(_file, RC_RenameCSFiles(Path.GetDirectoryName(_file)));
		}

		private static Dictionary<string, string> RC_RenameCSFiles(string projectDir)
		{
			Dictionary<string, string> renamedCSFilePairs = SCommon.CreateDictionaryIgnoreCase<string>();

			string homeDir = Directory.GetCurrentDirectory();
			try
			{
				Directory.SetCurrentDirectory(projectDir);

				foreach (string f_file in Directory.GetFiles(".", "*", SearchOption.AllDirectories))
				{
					string file = f_file;

					Console.WriteLine("RC_file: " + file);

					if (!SCommon.EndsWithIgnoreCase(file, ".cs")) // ? .cs ではない -> 除外
						continue;

					file = SCommon.MakeFullPath(file);
					file = SCommon.ChangeRoot(file, projectDir);

					if (SCommon.StartsWithIgnoreCase(file, "Properties\\")) // ? Properties フォルダの配下 -> 除外
						continue;

					string nameOld = file.Substring(0, file.Length - 3); // .cs を除去
					string nameNew = RC_CreateName();

					Action<string> a_rename = suffix =>
					{
						string fileOld = nameOld + suffix;
						string fileNew = nameNew + suffix;

						if (!File.Exists(fileOld))
							return;

						File.Move(fileOld, fileNew);

						renamedCSFilePairs.Add(fileOld, fileNew);
					};

					a_rename(".cs");
					a_rename(".Designer.cs");
					a_rename(".resx");
				}
			}
			finally
			{
				Directory.SetCurrentDirectory(homeDir);
			}

			return renamedCSFilePairs;
		}

		private static string RC_CreateName()
		{
			// crand 128 bit -> 重複を想定しない。

			return
				"Elsa_" +
				SCommon.CRandom.GetUInt64().ToString("D20") + "_de_" +
				SCommon.CRandom.GetUInt64().ToString("D20") +
				"_Sica";
		}

		private static void RC_RenameCompiles(string projectFile, Dictionary<string, string> renamedCSFilePairs)
		{
			string[] lines = File.ReadAllLines(projectFile, Encoding.UTF8);

			for (int index = 0; index < lines.Length; index++)
			{
				string line = lines[index];
				string[] parts = SCommon.ParseEnclosed(line, "<Compile Include=\"", "\"");

				if (parts == null)
					continue;

				string fileOld = parts[2];

				Console.WriteLine("RC_fileOld: " + fileOld);

				if (!renamedCSFilePairs.ContainsKey(fileOld))
				{
					Console.WriteLine("名前を変更していない。");
					continue;
				}
				string fileNew = renamedCSFilePairs[fileOld];

				line = parts[0] + parts[1] + fileNew + parts[3] + parts[4];
				lines[index] = line;
			}
			File.WriteAllLines(projectFile, lines, Encoding.UTF8);
		}
	}
}
