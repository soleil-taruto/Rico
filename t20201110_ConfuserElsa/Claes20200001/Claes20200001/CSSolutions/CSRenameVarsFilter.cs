using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.CSSolutions
{
	public class CSRenameVarsFilter
	{
		private string[] 予約語リスト = SCommon.TextToLines(CSResources.予約語リスト)
			.Select(v => v.Trim())
			.Where(v => v != "" && v[0] != ';') // ? 空行ではない && コメント行ではない
			.ToArray();

		private Dictionary<string, string> 変換テーブル = SCommon.CreateDictionary<string>();

		public string Filter(string name)
		{
			if (
				name == "" ||
				SCommon.DECIMAL.Contains(name[0]) ||
				this.予約語リスト.Contains(name)
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
			while (this.CNN_Names.ContainsKey(nameNew) || this.予約語リスト.Contains(nameNew));

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
	}
}
