using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
{
	public static class Ground
	{
		/// <summary>
		/// スナップショットしたファイル(又はフォルダ)の格納先のルートフォルダ
		/// 存在しない場合は、(安全のため)手動で作成すること。
		/// </summary>
		public static string SnapshotDir = @"C:\be\Snapshots";
	}
}
