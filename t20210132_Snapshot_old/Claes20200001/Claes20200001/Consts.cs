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
		/// スナップショットしたファイル(又はフォルダ)の格納先のルートフォルダ
		/// 存在しない場合は、(安全のため)手動で作成すること。
		/// </summary>
		public const string SNAPSHOT_DIR = @"C:\be\Snapshots";

		/// <summary>
		/// MAX_PATH == 256 を想定したパス長の上限
		/// フルパスの SJIS でのバイト列がこのバイト数を超えないようにする。
		/// 安全のため MAX_PATH より少し短くしておく
		/// C:\Factory\Common\DataConv.h の PATH_SIZE を元にした。
		/// </summary>
		public const int PATH_MAX = 250;
	}
}
