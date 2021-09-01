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
		/// ダウンロード・ルートDIR
		/// </summary>
		public const string DOWNLOAD_ROOT_DIR = @"C:\be\Web\DocRoot";

		/// <summary>
		/// 開発環境のルートDIR
		/// </summary>
		public const string DEV_ROOT_DIR = @"C:\Dev";

		/// <summary>
		/// データJSシグネチャ
		/// これに一致しない場合は処理しない。
		/// </summary>
		public const string DATA_JS_SIGNATURE = "ccsp-data_v001";

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

		/// <summary>
		/// ダウンロードURLのプリフィックス
		/// </summary>
		public const string DOWNLOAD_URL_PREFIX = "http://ornithopter.ccsp.mydns.jp:58946/anemoscope/";
	}
}
