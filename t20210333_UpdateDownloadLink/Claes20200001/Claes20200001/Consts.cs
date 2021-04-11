using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
{
	public static class Consts
	{
		/// <summary>
		/// 特設ページのルートDIR
		/// </summary>
		public const string SPEC_ROOT_DIR = @"C:\be\Web\SpecRoot";

		/// <summary>
		/// ダウンロード先のルートDIR
		/// </summary>
		public const string DOC_ROOT_DIR = @"C:\be\Web\DocRoot";

		/// <summary>
		/// ダウンロードリンクの開始パターン
		/// </summary>
		public const string LINK_START = "href=\"http://ornithopter.ccsp.mydns.jp:58946/anemoscope/";

		/// <summary>
		/// ダウンロードリンクの終了パターン
		/// </summary>
		public const string LINK_END = "\"";

		/// <summary>
		/// HTMLファイルのエンコーディング
		/// </summary>
		public static readonly Encoding HTML_ENCODING = Encoding.UTF8;
	}
}
