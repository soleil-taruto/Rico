using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using Charlotte.Commons;

namespace Charlotte
{
	public class VoyagerDistance
	{
		private const string NASA_DISTANCE_DATA_URL = "https://voyager.jpl.nasa.gov/assets/javascripts/distance_data.js";
		//private const string NASA_DISTANCE_DATA_URL = "http://stackprobe.ccsp.mydns.jp/index.html"; // test test test test test

		public class DistanceInfo
		{
			public SCommon.SimpleDateTime DateTime;
			public double Km;

			public string Serialize()
			{
				return this.DateTime.ToSec() + ":" + this.Km.ToString("F9");
			}

			public void Deserialize(string serializedString)
			{
				string[] lines = serializedString.Split(':');
				int c = 0;

				this.DateTime = new SCommon.SimpleDateTime(long.Parse(lines[c++]));
				this.Km = double.Parse(lines[c++]);

				if (c != lines.Length)
					throw new Exception("[VD]データ破損");
			}
		}

		public class DistancePairInfo
		{
			public DistanceInfo A;
			public DistanceInfo B;

			public double GetKm(SCommon.SimpleDateTime dt)
			{
				double rateOfDateTime = P_RateAToB(this.A.DateTime.ToSec(), this.B.DateTime.ToSec(), dt.ToSec());
				double km = P_AToBRate(this.A.Km, this.B.Km, rateOfDateTime);
				return km;
			}

			public string Serialize()
			{
				return this.A.Serialize() + "/" + this.B.Serialize();
			}

			public void Deserialize(string serializedString)
			{
				string[] lines = serializedString.Split('/');
				int c = 0;

				this.A = new DistanceInfo();
				this.B = new DistanceInfo();

				this.A.Deserialize(lines[c++]);
				this.B.Deserialize(lines[c++]);

				if (c != lines.Length)
					throw new Exception("[VD]データ破損");
			}
		}

		public DistancePairInfo Earth_Voyager_1;
		public DistancePairInfo Earth_Voyager_2;
		public DistancePairInfo Sun_Voyager_1;
		public DistancePairInfo Sun_Voyager_2;

		public SCommon.SimpleDateTime データ取得日時;

		public VoyagerDistance()
		{
			const string MUTEX_NAME = "{ae070e1c-b84b-4f52-a157-a2911666e97b}"; // shared_uuid@g

			ProcMain.WriteLog("[VD].1");

			using (Mutex mutex = new Mutex(false, MUTEX_NAME))
			{
				mutex.WaitOne();
				try
				{
					ProcMain.WriteLog("[VD].2");
					this.Load();
					ProcMain.WriteLog("[VD].3");
				}
				finally
				{
					mutex.ReleaseMutex();
				}
			}
			ProcMain.WriteLog("[VD].4");
		}

		private void Load()
		{
			if (!this.LoadFromFile(false)) // ? データ読み込み失敗 || キャッシュ期限切れ
			{
				try
				{
					P_LoopTry(this.GetNasaData, 3);
					this.SaveToFile();
				}
				catch
				{
					if (!this.LoadFromFile(true))
						throw new Exception("[VD]データ読み込み失敗");
				}
			}
		}

		private static void P_LoopTry(Action action, int tryCountMax)
		{
			for (int tryCount = 1; ; tryCount++)
			{
				try
				{
					action();
					return;
				}
				catch (Exception ex)
				{
					ProcMain.WriteLog("ex: " + ex + " (処理続行)");

					if (tryCountMax <= tryCount)
						throw;
				}
			}
		}

		private void GetNasaData()
		{
			ProcMain.WriteLog("[VD]データ(再)取得");

			using (WorkingDir wd = new WorkingDir())
			{
				SCommon.Batch(new string[] { "curl -o out.txt " + NASA_DISTANCE_DATA_URL }, wd.GetPath("."));

				string[] lines = File.ReadAllLines(wd.GetPath("out.txt"), Encoding.ASCII);
				int c = 0;

				long epoch_0 = long.Parse(ParseValue(lines[c++], "let epoch_0 = ", ";"));
				long epoch_1 = long.Parse(ParseValue(lines[c++], "let epoch_1 = ", ";"));
				CheckEmpty(lines[c++]);
				double dist_0_v1 = double.Parse(ParseValue(lines[c++], "let dist_0_v1 = ", ";"));
				double dist_1_v1 = double.Parse(ParseValue(lines[c++], "let dist_1_v1 = ", ";"));
				CheckEmpty(lines[c++]);
				double dist_0_v2 = double.Parse(ParseValue(lines[c++], "let dist_0_v2 = ", ";"));
				double dist_1_v2 = double.Parse(ParseValue(lines[c++], "let dist_1_v2 = ", ";"));
				CheckEmpty(lines[c++]);
				double dist_0_v1s = double.Parse(ParseValue(lines[c++], "let dist_0_v1s = ", ";"));
				double dist_1_v1s = double.Parse(ParseValue(lines[c++], "let dist_1_v1s = ", ";"));
				CheckEmpty(lines[c++]);
				double dist_0_v2s = double.Parse(ParseValue(lines[c++], "let dist_0_v2s = ", ";"));
				double dist_1_v2s = double.Parse(ParseValue(lines[c++], "let dist_1_v2s = ", ";"));

				if (c != lines.Length)
					throw new Exception("[VD]FORMAT_ERROR");

				long GMT_TO_JST = 9 * 3600;

				SCommon.SimpleDateTime dateTime_0 = new SCommon.SimpleDateTime(epoch_0 + SCommon.TimeStampToSec.ToSec(19700101000000) + GMT_TO_JST);
				SCommon.SimpleDateTime dateTime_1 = new SCommon.SimpleDateTime(epoch_1 + SCommon.TimeStampToSec.ToSec(19700101000000) + GMT_TO_JST);

				this.Earth_Voyager_1 = new DistancePairInfo()
				{
					A = new DistanceInfo() { DateTime = dateTime_0, Km = dist_0_v1 },
					B = new DistanceInfo() { DateTime = dateTime_1, Km = dist_1_v1 },
				};
				this.Earth_Voyager_2 = new DistancePairInfo()
				{
					A = new DistanceInfo() { DateTime = dateTime_0, Km = dist_0_v2 },
					B = new DistanceInfo() { DateTime = dateTime_1, Km = dist_1_v2 },
				};
				this.Sun_Voyager_1 = new DistancePairInfo()
				{
					A = new DistanceInfo() { DateTime = dateTime_0, Km = dist_0_v1s },
					B = new DistanceInfo() { DateTime = dateTime_1, Km = dist_1_v1s },
				};
				this.Sun_Voyager_2 = new DistancePairInfo()
				{
					A = new DistanceInfo() { DateTime = dateTime_0, Km = dist_0_v2s },
					B = new DistanceInfo() { DateTime = dateTime_1, Km = dist_1_v2s },
				};

				this.データ取得日時 = SCommon.SimpleDateTime.Now();
			}
		}

		private static string ParseValue(string line, string leader, string trailer)
		{
			if (!line.StartsWith(leader))
				throw new Exception("[VD]FORMAT_ERROR");

			line = line.Substring(leader.Length);

			if (!line.EndsWith(trailer))
				throw new Exception("[VD]FORMAT_ERROR");

			line = line.Substring(0, line.Length - trailer.Length);
			line = line.Trim();
			return line;
		}

		private static void CheckEmpty(string line)
		{
			if (line != "")
				throw new Exception("[VD]FORMAT_ERROR");
		}

		private string[] Serialize()
		{
			return new string[]
			{
				"" + this.データ取得日時.ToSec(),
				this.Earth_Voyager_1.Serialize(),
				this.Earth_Voyager_2.Serialize(),
				this.Sun_Voyager_1.Serialize(),
				this.Sun_Voyager_2.Serialize(),
			};
		}

		private void Deserialize(string[] serializedLines)
		{
			string[] lines = serializedLines;
			int c = 0;

			this.データ取得日時 = new SCommon.SimpleDateTime(long.Parse(lines[c++]));

			this.Earth_Voyager_1 = new DistancePairInfo();
			this.Earth_Voyager_2 = new DistancePairInfo();
			this.Sun_Voyager_1 = new DistancePairInfo();
			this.Sun_Voyager_2 = new DistancePairInfo();

			this.Earth_Voyager_1.Deserialize(lines[c++]);
			this.Earth_Voyager_2.Deserialize(lines[c++]);
			this.Sun_Voyager_1.Deserialize(lines[c++]);
			this.Sun_Voyager_2.Deserialize(lines[c++]);

			if (c != lines.Length)
				throw new Exception("[VD]データ破損");
		}

		private const string NASA_DATA_FILE = @"C:\appdata\VoyagerDistance.txt"; // zantei

		private void SaveToFile()
		{
			File.WriteAllLines(NASA_DATA_FILE, this.Serialize(), Encoding.ASCII);
		}

		private const long CACHE_TIMEOUT_SEC = 60;

		private bool LoadFromFile(bool allowTimeout)
		{
			if (!File.Exists(NASA_DATA_FILE))
			{
				ProcMain.WriteLog("[VD]データファイル無し");
				return false;
			}

			try
			{
				this.Deserialize(File.ReadAllLines(NASA_DATA_FILE, Encoding.ASCII));
			}
			catch (Exception ex)
			{
				ProcMain.WriteLog("[VD]データファイル破損 : " + ex + " (処理続行)");
				return false;
			}

			if (this.データ取得日時.ToSec() + CACHE_TIMEOUT_SEC <= SCommon.SimpleDateTime.Now().ToSec())
			{
				if (allowTimeout)
				{
					ProcMain.WriteLog("[VD]キャッシュ期限切れ -> 無視");
				}
				else
				{
					ProcMain.WriteLog("[VD]キャッシュ期限切れ");
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// 始点から終点までの間の指定レートの位置の値を返す。
		/// </summary>
		/// <param name="a">始点</param>
		/// <param name="b">終点</param>
		/// <param name="rate">レート</param>
		/// <returns>レートの値</returns>
		private static double P_AToBRate(double a, double b, double rate)
		{
			return a + (b - a) * rate;
		}

		/// <summary>
		/// 始点から終点までの間の位置をレートに変換する。
		/// </summary>
		/// <param name="a">始点</param>
		/// <param name="b">終点</param>
		/// <param name="value">位置</param>
		/// <returns>レート</returns>
		private static double P_RateAToB(double a, double b, double value)
		{
			return (value - a) / (b - a);
		}
	}
}
