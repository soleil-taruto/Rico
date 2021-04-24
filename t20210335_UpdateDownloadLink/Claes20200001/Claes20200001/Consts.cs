using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte
{
	public static class Consts
	{
		/// <summary>
		/// 特設ページのルートDIR
		/// </summary>
		public const string SPEC_ROOT_DIR = @"C:\be\Web\SpecRoot";

		/// <summary>
		/// データJSのローカルファイル名
		/// </summary>
		public const string DATA_JS_LOCAL_NAME = "ccsp-data.js";

		/// <summary>
		/// データJSのエンコーディング
		/// </summary>
		public static readonly Encoding DATA_JS_ENCODING = Encoding.ASCII;

		/// <summary>
		/// ダウンロード対象ファイルのサフィックス
		/// </summary>
		public const string DOWNLOAD_FILE_SUFFIX = ".zip";
	}
}
