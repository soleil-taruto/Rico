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
	}
}
