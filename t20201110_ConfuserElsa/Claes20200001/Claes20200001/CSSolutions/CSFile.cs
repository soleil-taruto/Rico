using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;

namespace Charlotte.CSSolutions
{
	public class CSFile
	{
		private string _file;

		public CSFile(string file)
		{
			_file = file;
		}

		public string GetFile()
		{
			return _file;
		}

		public void SolveNamespace()
		{
			string[] lines = File.ReadAllLines(_file, Encoding.UTF8);

			for (int index = 0; index < lines.Length; index++)
			{
				if (lines[index].StartsWith("using Charlotte."))
					lines[index] = "";

				if (lines[index].StartsWith("namespace Charlotte."))
					lines[index] = "namespace Charlotte";
			}
			File.WriteAllLines(_file, lines, Encoding.UTF8);
		}

		public void RemoveComments()
		{
			char[] chrs = File.ReadAllText(_file, Encoding.UTF8).ToArray();

			for (int index = 0; index < chrs.Length; index++)
			{
				// ? プリプロセッサ
				if (chrs[index] == '#')
				{
					// 事故防止のためこの行のこの文字以降については危ない文字を消しておく。
					// コメントを削除する必要はあるので、index は進めない。

					for (int c = index + 1; chrs[c] != '\n'; c++)
					{
						if (
							chrs[c] == '\'' ||
							chrs[c] == '"'
							)
							chrs[c] = '_';
					}
				}
				// ? リテラル文字
				else if (chrs[index] == '\'')
				{
					index++;

					if (chrs[index] == '\\')
					{
						index++;

						if (chrs[index] == 'u')
							index += 4;
					}
					index++;

					if (chrs[index] != '\'')
						throw null; // 想定外
				}
				// ? (改行可能な)リテラル文字列
				else if (
					chrs[index + 0] == '@' &&
					chrs[index + 1] == '"'
					)
				{
					index = RC_Skip(chrs, index + 2, '"', '"', '"', '"', '"');
				}
				// ? リテラル文字列
				else if (chrs[index] == '"')
				{
					index = RC_Skip(chrs, index + 1, '\\', '\\', '\\', '"', '"');
				}
				// ? C系コメント
				else if (
					chrs[index + 0] == '/' &&
					chrs[index + 1] == '*'
					)
				{
					chrs[index + 0] = ' '; // '/' 除去
					chrs[index + 1] = ' '; // '*' 除去

					index = RC_Mask(chrs, index + 2, '*', '/');

					chrs[index + 0] = ' '; // '*' 除去
					chrs[index + 1] = ' '; // '/' 除去

					index++;
				}
				// ? C++系コメント
				else if (
					chrs[index + 0] == '/' &&
					chrs[index + 1] == '/'
					)
				{
					chrs[index + 0] = ' '; // '/' 除去
					chrs[index + 1] = ' '; // '/' 除去

					// C++系コメントの後に改行が必ずあると想定する。

					index = RC_Mask(chrs, index + 2, '\r', '\n') + 1;
				}
			}
			File.WriteAllText(_file, new string(chrs), Encoding.UTF8);

			// コメント除去後に生じうる行末の空白を除去する。
			{
				string[] lines = File.ReadAllLines(_file, Encoding.UTF8);

				lines = lines.Select(v => v.TrimEnd()).ToArray();

				File.WriteAllLines(_file, lines, Encoding.UTF8);
			}
		}

		private static int RC_Skip(char[] chrs, int index, char ignChr_a1, char ignChr_a2, char ignChr_b1, char ignChr_b2, char endChr)
		{
			for (; ; index++)
			{
				while (
					chrs[index + 0] == ignChr_a1 &&
					chrs[index + 1] == ignChr_a2
					||
					chrs[index + 0] == ignChr_b1 &&
					chrs[index + 1] == ignChr_b2
					)
					index += 2;

				if (chrs[index] == endChr)
					break;
			}
			return index; // endChr の位置
		}

		private static int RC_Mask(char[] chrs, int index, char endChr_1, char endChr_2)
		{
			while (
				chrs[index + 0] != endChr_1 ||
				chrs[index + 1] != endChr_2
				)
				chrs[index++] = ' ';

			return index; // endChr_1 の位置
		}

		public void RemovePreprocessorDirectives()
		{
			string[] lines = File.ReadAllLines(_file, Encoding.UTF8);

			for (int index = 0; index < lines.Length; index++)
			{
				string line = lines[index];

				if (
					line == "#if true" ||
					line == "#if !false"
					)
				{
					lines[index] = "";
					index = RPD_RemoveIfTrue(lines, index + 1);
				}
				else if (
					line == "#if !true" ||
					line == "#if false"
					)
				{
					lines[index] = "";
					index = RPD_RemoveIfFalse(lines, index + 1);
				}
				else if (line.StartsWith("#")) // ? #if-系
				{
					throw new Exception("Bad #...");
				}
				else if (line.TrimStart().StartsWith("#")) // ? #region-系
				{
					lines[index] = "";
				}
			}
			File.WriteAllLines(_file, lines, Encoding.UTF8);
		}

		private int RPD_RemoveIfFalse(string[] lines, int index)
		{
			for (; ; index++)
			{
				string line = lines[index];

				if (
					line == "#else" ||
					line == "#elif true" ||
					line == "#elif !false"
					)
				{
					lines[index] = "";
					index = RPD_RemoveIfTrue(lines, index + 1);
					break;
				}
				else if (
					line == "#elif !true" ||
					line == "#elif false"
					)
				{
					// このまま続ける。
				}
				else if (line == "#endif")
				{
					lines[index] = "";
					break;
				}
				else if (line.StartsWith("#")) // ? #if-系
				{
					throw new Exception("Bad #...");
				}
				lines[index] = "";
			}
			return index;
		}

		private int RPD_RemoveIfTrue(string[] lines, int index)
		{
			for (; ; index++)
			{
				string line = lines[index];

				if (
					line == "#else" ||
					line == "#elif true" ||
					line == "#elif !false" ||
					line == "#elif !true" ||
					line == "#elif false"
					)
				{
					lines[index] = "";
					break;
				}
				else if (line == "#endif")
				{
					lines[index] = "";
					goto endFunc;
				}
				else if (line.StartsWith("#")) // ? #if-系
				{
					throw new Exception("Bad #...");
				}
				else if (line.TrimStart().StartsWith("#")) // ? #region-系
				{
					lines[index] = "";
				}
			}
			for (; ; index++)
			{
				string line = lines[index];

				if (line == "#endif")
				{
					lines[index] = "";
					break;
				}
				else if (line.StartsWith("#")) // ? #if-系
				{
					// noop -- 無視する。
				}
				lines[index] = "";
			}
		endFunc:
			return index;
		}

		public void SolveAccessModifiers()
		{
			string text = File.ReadAllText(_file, Encoding.UTF8);

			text = SAM_Replace(text, "{ get; private set; }", ";");
			text = SAM_Replace(text, "{ private get; set; }", ";");
			text = SAM_Replace(text, "private const", "public static");
			text = SAM_Replace(text, "protected const", "public static");
			text = SAM_Replace(text, "public const", "public static");
			text = SAM_Replace(text, "const", "");
			//text = SAM_Replace(text, "private", "public"); // 継承クラスに同名のメンバが居るとマズい。
			//text = SAM_Replace(text, "protected", "public"); // protected override に対応していない。
			text = SAM_Replace(text, "public static class", "public class");
			text = SAM_Replace(text, "static class Program", "public class Program"); // Program.cs 専用

			File.WriteAllText(_file, text, Encoding.UTF8);
		}

		private string SAM_Replace(string text, string targPtn, string destPtn)
		{
			for (int index = 0; ; )
			{
				int next = text.IndexOf(targPtn, index);

				if (next == -1)
					break;

				if (
					SAM_IsSpaceOrPunct(text[next - 1]) &&
					SAM_IsSpaceOrPunct(text[next + targPtn.Length])
					)
					text = text.Substring(0, next) + destPtn + text.Substring(next + targPtn.Length);

				index = next + destPtn.Length;
			}
			return text;
		}

		private static bool SAM_IsSpaceOrPunct(char chr)
		{
			return !Common.IsCSWordChar(chr);
		}

		public void SolveLiteralStrings()
		{
			this.SolveLiteralStrings_01();
			this.SolveLiteralStrings_02();
		}

		private void SolveLiteralStrings_01()
		{
			char[] chrs = File.ReadAllText(_file, Encoding.UTF8).ToArray();
			StringBuilder dest = new StringBuilder();

			for (int index = 0; index < chrs.Length; index++)
			{
				// ? リテラル文字
				if (chrs[index] == '\'')
				{
					char chr;

					index++;

					if (chrs[index] == '\\')
					{
						index++;

						if (chrs[index] == '\\')
						{
							chr = '\\';
						}
						else if (chrs[index] == 't')
						{
							chr = '\t';
						}
						else if (chrs[index] == 'r')
						{
							chr = '\r';
						}
						else if (chrs[index] == 'n')
						{
							chr = '\n';
						}
						else if (chrs[index] == 'u')
						{
							chr = (char)Convert.ToUInt16(new string(new char[]
							{
								chrs[index + 1],
								chrs[index + 2],
								chrs[index + 3],
								chrs[index + 4],
							}),
							16
							);

							index += 4;
						}
						else
						{
							throw null; // 想定外
						}
					}
					else
					{
						chr = chrs[index];
					}
					index++;

					if (chrs[index] != '\'')
						throw null; // 想定外

					dest.Append("(char)0x" + ((uint)chr).ToString("x4"));
				}
				// ? (改行可能な)リテラル文字列
				else if (
					chrs[index + 0] == '@' &&
					chrs[index + 1] == '"'
					)
				{
					dest.Append('"');
					index += 2;

					for (; ; index++)
					{
						char chr = chrs[index];

						if (chr == '"')
						{
							if (chrs[index + 1] != '"')
								break;

							index++;
						}
						dest.Append("\\u");
						dest.Append(((ushort)chr).ToString("x4"));
					}
					dest.Append('"');
				}
				else if (chrs[index] == '"') // ? リテラル文字列
				{
					dest.Append('"');
					index++;

					for (; ; index++)
					{
						char chr = chrs[index];

						if (chr == '"')
							break;

						if (chr == '\\')
						{
							index++;
							chr = chrs[index];

							if (chr == 't')
								chr = '\t';
							else if (chr == 'r')
								chr = '\r';
							else if (chr == 'n')
								chr = '\n';
						}
						dest.Append("\\u");
						dest.Append(((ushort)chr).ToString("x4"));
					}
					dest.Append('"');
				}
				else
				{
					dest.Append(chrs[index]);
				}
			}
			File.WriteAllText(_file, dest.ToString(), Encoding.UTF8);
		}

		private void SolveLiteralStrings_02()
		{
			string[] lines = File.ReadAllLines(_file, Encoding.UTF8);
			List<string> varLines = new List<string>();

			for (int index = 0; index < lines.Length; index++)
			{
				string line = lines[index];
				string tsLine = line.TrimStart();

				// 以下のリテラル文字列は変数化出来ない。
				// -- switch の case
				// -- デフォルト引数
				// -- [DllImport(... とか

				if (tsLine.StartsWith("case ")) // ? switch の case
				{
					// noop
				}
				else if (tsLine.StartsWith("private "))
				{
					// noop
				}
				else if (tsLine.StartsWith("protected "))
				{
					// noop
				}
				else if (tsLine.StartsWith("public "))
				{
					// noop
				}
				else if (tsLine.StartsWith("[")) // ? [DllImport(... とか
				{
					// noop
				}
				else
				{
					for (; ; )
					{
						int c = line.IndexOf('"');

						if (c == -1)
							break;

						int c2 = line.IndexOf('"', c + 1);

						if (c2 == -1)
							throw null; // never

						c2++;
						string varName = SL2_CreateVarName();
						varLines.Add("\t\tpublic static string " + varName + " { get { return " + line.Substring(c, c2 - c) + "; }}");
						//varLines.Add("\t\tpublic static string " + varName + " = " + line.Substring(c, c2 - c) + ";");
						line = line.Substring(0, c) + varName + line.Substring(c2);
					}
					lines[index] = line;
				}
			}
			File.WriteAllLines(_file, this.SL2_クラスの先頭に挿入(lines, varLines), Encoding.UTF8);
		}

		private static string SL2_CreateVarName()
		{
			// crand 128 bit -> 重複を想定しない。

			return
				"SL2_a_" +
				SCommon.CRandom.GetUInt64().ToString("D20") + "_" +
				SCommon.CRandom.GetUInt64().ToString("D20") +
				"_z";
		}

		private IEnumerable<string> SL2_クラスの先頭に挿入(string[] lines, List<string> varLines)
		{
			if (varLines.Count == 0)
				return lines;

			int index = SCommon.IndexOf(lines, v => v.StartsWith("\t\t"));

			if (index == -1)
				throw new Exception("クラスの先頭を見つけられませんでした。");

			return lines.Take(index).Concat(varLines).Concat(lines.Skip(index));
		}

		public void AddDummyMember()
		{
			string[] lines = File.ReadAllLines(_file, Encoding.UTF8);

			if (ADM_IsStruct(lines)) // ? 構造体
				return;

			string[] dmLines = SCommon.TextToLines(CSResources.DUMMY_MEMBER).Where(v => v != "").ToArray();
			int end = ADM_GetClassEnd(lines);

			if (end == -1)
				return;

			int dmCount = lines.Length / dmLines.Length;
			dmCount /= 3;
			dmCount++;

			// DM-生成＆挿入
			{
				List<string> dest = new List<string>();

				for (int index = 0; index < dmCount; index++)
				{
					string ident = ADM_CreateIdent();

					foreach (string f_line in dmLines)
					{
						string line = f_line;
						line = line.Replace("$$", ident);
						dest.Add(line);
					}
				}
				lines = lines.Take(end).Concat(dest).Concat(lines.Skip(end)).ToArray();
			}

			File.WriteAllLines(_file, lines, Encoding.UTF8);
		}

		private static bool ADM_IsStruct(string[] lines)
		{
			return lines.Any(v => v.StartsWith("\tpublic struct "));
		}

		private static int ADM_GetClassEnd(string[] lines)
		{
			return SMO_GetClassEnd(lines);
		}

		private static string ADM_CreateIdent()
		{
			// crand 128 bit -> 重複を想定しない。

			return
				"ADM_a_" +
				SCommon.CRandom.GetUInt64().ToString("D20") + "_" +
				SCommon.CRandom.GetUInt64().ToString("D20") +
				"_z";
		}

		public void RenameEx(Func<string, string> filter)
		{
			string text = File.ReadAllText(_file, Encoding.UTF8);
			string text_bk = text;
			bool insideOfLiteralChar = false;
			bool insideOfLiteralString = false;

			for (int index = 0; index < text.Length; index++)
			{
				if (text[index] == '\\') // ? エスケープ文字 -> スキップする。
				{
					index++;
					continue;
				}
				insideOfLiteralChar ^= text[index] == '\'';
				insideOfLiteralString ^= text[index] == '"';

				if (
					!insideOfLiteralChar &&
					!insideOfLiteralString &&
					Common.IsCSWordChar(text[index])
					)
				{
					int end = index + 1;

					// HACK: ソースファイルは改行(単語ではない)で終わっている想定になっている。

					while (Common.IsCSWordChar(text[end]))
						end++;

					string name = text.Substring(index, end - index);
					string nameNew = filter(name);

					if (name == nameNew)
					{
						index = end;
					}
					else
					{
						text = text.Substring(0, index) + nameNew + text.Substring(end);
						index += nameNew.Length;
					}
				}
			}

			text = RX_Mix(text, text_bk);

			File.WriteAllText(_file, text, Encoding.UTF8);
		}

		private static string RX_Mix(string text, string text_bk)
		{
			string[] lines = SCommon.TextToLines(text);
			string[] lines_bk = SCommon.TextToLines(text_bk);

			if (lines.Length != lines_bk.Length) // 同じはず
				throw null; // 想定外

			List<string> dest = new List<string>();

			for (int index = 0; index < lines.Length; index++)
			{
				dest.Add(lines[index]);
				dest.Add("// " + lines_bk[index]);
			}
			return SCommon.LinesToText(dest.ToArray());
		}

		private class SMO_RangeInfo
		{
			public int Start;
			public int End;
			public bool HoldingOrder;
		}

		public void ShuffleMemberOrder()
		{
			string[] lines = File.ReadAllLines(_file, Encoding.UTF8);

			int start;
			int end = SMO_GetClassEnd(lines);

			if (end == -1) // ? クラス閉じが見つからない。-> メンバーが無いのかも？終了する。
			{
				Console.WriteLine("シャッフル・キャンセル -- クラス閉じが見つからない。");
				return;
			}
			List<SMO_RangeInfo> ranges = new List<SMO_RangeInfo>();
			bool dllImportFlag = false;

			for (int index = 0; index < end; index++)
			{
				string line = lines[index];
				bool foundMemberFlag = false;

				if (line.StartsWith("\t\t[")) // ? [DllImport(... とか
				{
					if (!dllImportFlag)
					{
						foundMemberFlag = true;
						dllImportFlag = true;
					}
				}
				else if (
					line.StartsWith("\t\tprivate ") ||
					line.StartsWith("\t\tprotected ") ||
					line.StartsWith("\t\tpublic ") ||
					line.StartsWith("\t\tstatic ") // static void Main() とか
					)
				{
					if (!dllImportFlag)
						foundMemberFlag = true;
					else
						dllImportFlag = false;
				}

				if (foundMemberFlag)
				{
					// クラスのメンバーを追加
					ranges.Add(new SMO_RangeInfo()
					{
						Start = index,
						End = -1, // ダミー
					});
				}
			}
			if (ranges.Count <= 1) // ? メンバーが１つ以下 -> シャッフル不要
			{
				Console.WriteLine("シャッフル・キャンセル -- メンバーが１つ以下");
				return;
			}
			start = ranges[0].Start;

			for (int index = 1; index < ranges.Count; index++)
				ranges[index - 1].End = ranges[index].Start;

			ranges[ranges.Count - 1].End = end;

			// 最初の行に = があったら初期化有りフィールドと見なして、初期化順序を維持するために順序の入れ替えを抑制する。
			// -- デフォルトの引数有りメソッドも引っ掛かってしまうけど、とりあえずはこれでいいや..
			//
			foreach (SMO_RangeInfo range in ranges)
				range.HoldingOrder = lines[range.Start].Contains("=");

			// シャッフル
			{
				List<SMO_RangeInfo> hoRanges = new List<SMO_RangeInfo>();
				List<SMO_RangeInfo> unhoRanges = new List<SMO_RangeInfo>();

				foreach (SMO_RangeInfo range in ranges)
				{
					if (range.HoldingOrder)
						hoRanges.Add(range);
					else
						unhoRanges.Add(range);
				}
				ranges = hoRanges;
				hoRanges = null;

				foreach (SMO_RangeInfo range in unhoRanges)
					ranges.Insert(SCommon.CRandom.GetInt(ranges.Count + 1), range);
			}

			// ファイルの先頭
			ranges.Insert(0, new SMO_RangeInfo()
			{
				Start = 0,
				End = start,
			});

			// ファイルの終端
			ranges.Add(new SMO_RangeInfo()
			{
				Start = end,
				End = lines.Length,
			});

			File.WriteAllLines(_file, SMO_GetOrderLines(lines, ranges), Encoding.UTF8);
		}

		private static int SMO_GetClassEnd(string[] lines)
		{
			for (int index = lines.Length - 1; 0 <= index; index--)
				if (lines[index].StartsWith("\t}")) // ? クラス閉じ
					return index;

			return -1; // not found
		}

		private static IEnumerable<string> SMO_GetOrderLines(string[] lines, IEnumerable<SMO_RangeInfo> ranges)
		{
			foreach (SMO_RangeInfo range in ranges)
			{
				yield return "// HO=" + range.HoldingOrder;

				for (int index = range.Start; index < range.End; index++)
					yield return lines[index];
			}
		}
	}
}
