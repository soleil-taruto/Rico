﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.CSSolutions
{
	public class CSRenameVarsFilter
	{
		/// <summary>
		/// テスト用
		/// null == 無効
		/// null 以外 == この文字列を「置き換え禁止ワードのリスト」から除去する。
		/// </summary>
		public static string 置き換え禁止ワードの例外ワード = null;

		/// <summary>
		/// テスト用
		/// </summary>
		/// <returns>置き換え禁止ワードのリスト</returns>
		public string[] Get置き換え禁止ワードのリスト()
		{
			return this.置き換え禁止ワードのリスト;
		}

		private string[] 置き換え禁止ワードのリスト = SCommon.TextToLines(CSResources.予約語リスト + Consts.CRLF + CSResources.予約語クラス名リスト)
			.Select(v => v.Trim())
			.Where(v => v != "" && v[0] != ';') // ? 空行ではない && コメント行ではない
			.Where(v => v != 置き換え禁止ワードの例外ワード) // テスト用
			.ToArray();

		private Dictionary<string, string> 変換テーブル = SCommon.CreateDictionary<string>();

		public string Filter(string name)
		{
			if (
				name == "" ||
				SCommon.DECIMAL.Contains(name[0]) ||
				this.置き換え禁止ワードのリスト.Contains(name)
				)
				return name;

			string nameNew;

			if (this.変換テーブル.ContainsKey(name))
			{
				nameNew = this.変換テーブル[name];
			}
			else
			{
				nameNew = this.CreateNameNew();
				this.変換テーブル.Add(name, nameNew);
			}
			return nameNew;
		}

		private Dictionary<string, object> CNN_Names = SCommon.CreateDictionary<object>();

		public string CreateNameNew()
		{
			string nameNew;

			do
			{
				nameNew = this.TryCreateNameNew();
			}
			while (this.CNN_Names.ContainsKey(nameNew) || this.置き換え禁止ワードのリスト.Contains(nameNew));

			this.CNN_Names.Add(nameNew, null);
			return nameNew;
		}

		private string[] ランダムな単語リスト = SCommon.TextToLines(CSResources.ランダムな単語リスト)
			.Select(v => v.Trim())
			.Where(v => v != "" && v[0] != ';') // ? 空行ではない && コメント行ではない
			.ToArray();

		private string TryCreateNameNew()
		{
			StringBuilder buff = new StringBuilder();
			int count = SCommon.CRandom.GetRange(3, 5);

			for (int index = 0; index < count; index++)
				buff.Append(SCommon.CRandom.ChooseOne(this.ランダムな単語リスト));

			return buff.ToString();
		}

		private string[] 予約語クラス名リスト = SCommon.TextToLines(CSResources.予約語クラス名リスト)
			.Select(v => v.Trim())
			.Where(v => v != "" && v[0] != ';') // ? 空行ではない && コメント行ではない
			.ToArray();

		public bool Is予約語クラス名(string name)
		{
			return 予約語クラス名リスト.Contains(name);
		}
	}
}
