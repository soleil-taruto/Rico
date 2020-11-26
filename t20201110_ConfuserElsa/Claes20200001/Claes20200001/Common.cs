using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte
{
	public static class Common
	{
		/// <summary>
		/// C#の識別子に使用可能な文字か判定する。
		/// </summary>
		/// <param name="chr">判定する文字</param>
		/// <returns>C#の識別子に使用可能な文字か</returns>
		public static bool IsCSWordChar(char chr)
		{
			return
				SCommon.ALPHA.Contains(chr) ||
				SCommon.alpha.Contains(chr) ||
				SCommon.DECIMAL.Contains(chr) ||
				chr == '_' ||
				0x100 <= (uint)chr; // ? 日本語
		}

		public static bool IsHexadecimal(char chr)
		{
			return
				SCommon.HEXADECIMAL.Contains(chr) ||
				SCommon.hexadecimal.Contains(chr);
		}

#if false // 不使用
		public static string[] DivideTag(string text, string startTag, string endTag, int startIndex = 0, bool ignoreCase = false)
		{
			int start;
			int content;
			int end;
			int after;

			if (ignoreCase)
				start = text.ToLower().IndexOf(startTag.ToLower(), startIndex);
			else
				start = text.IndexOf(startTag, startIndex);

			if (start == -1)
				return null;

			content = start + startTag.Length;

			if (ignoreCase)
				end = text.ToLower().IndexOf(endTag.ToLower(), content);
			else
				end = text.IndexOf(endTag, content);

			if (end == -1)
				return null;

			after = end + endTag.Length;

			return new string[]
			{
				text.Substring(0, start),
				text.Substring(start, content - start),
				text.Substring(content, end - content),
				text.Substring(end, after - end),
				text.Substring(after),
			};
		}
#endif
	}
}
