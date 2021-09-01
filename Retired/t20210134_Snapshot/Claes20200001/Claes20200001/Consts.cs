using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;

namespace Charlotte
{
	public static class Consts
	{
		/// <summary>
		/// MAX_PATH == 256 を想定したパス長の上限
		/// フルパスの SJIS でのバイト列がこのバイト数を超えないようにする。
		/// 安全のため MAX_PATH より少し短くしておく
		/// C:\Factory\Common\DataConv.h の PATH_SIZE を元にした。
		/// </summary>
		public const int PATH_MAX = 250;

		/// <summary>
		/// スナップショットに関する情報ファイル
		/// 短い出力パスの時のみ、入力パス情報を記載して出力する。
		/// </summary>
		public const string TIPS_LOCAL_FILE = "____tips.txt";
	}
}
