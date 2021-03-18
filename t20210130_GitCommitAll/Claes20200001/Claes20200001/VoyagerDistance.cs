using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using System.IO;

// ^ sync @ VoyagerDistance

namespace Charlotte
{
	// sync > @ VoyagerDistance

	public class VoyagerDistance
	{
		private const string NASA_DISTANCE_DATA_URL = "https://voyager.jpl.nasa.gov/assets/javascripts/distance_data.js";

		public class DistanceInfo
		{
			public SCommon.SimpleDateTime DateTime;
			public double Km;
		}

		public class DistancePairInfo
		{
			public DistanceInfo A;
			public DistanceInfo B;

			public double GetKm(SCommon.SimpleDateTime dt)
			{
				double rateOfDateTime = Common.RateAToB(this.A.DateTime.ToSec(), this.B.DateTime.ToSec(), dt.ToSec());
				double km = Common.AToBRate(this.A.Km, this.B.Km, rateOfDateTime);
				return km;
			}
		}

		public DistancePairInfo Earth_Voyager_1;
		public DistancePairInfo Earth_Voyager_2;
		public DistancePairInfo Sun_Voyager_1;
		public DistancePairInfo Sun_Voyager_2;

		public VoyagerDistance()
		{
			try
			{
				this.LoadNasaData();
			}
			catch
			{
				try
				{
					this.LoadNasaData(); // リトライ_1回目
				}
				catch
				{
					this.LoadNasaData(); // リトライ_2回目
				}
			}
		}

		private void LoadNasaData()
		{
			using (WorkingDir wd = new WorkingDir())
			{
				SCommon.Batch(new string[] { "curl -o out.txt " + NASA_DISTANCE_DATA_URL }, wd.GetPath("."));

				string[] lines = File.ReadAllLines(wd.GetPath("out.txt"), Encoding.ASCII);
				int c = 0;

				long epoch_0 = long.Parse(ParseValue(lines[c++]));
				long epoch_1 = long.Parse(ParseValue(lines[c++]));
				c++;
				double dist_0_v1 = double.Parse(ParseValue(lines[c++]));
				double dist_1_v1 = double.Parse(ParseValue(lines[c++]));
				c++;
				double dist_0_v2 = double.Parse(ParseValue(lines[c++]));
				double dist_1_v2 = double.Parse(ParseValue(lines[c++]));
				c++;
				double dist_0_v1s = double.Parse(ParseValue(lines[c++]));
				double dist_1_v1s = double.Parse(ParseValue(lines[c++]));
				c++;
				double dist_0_v2s = double.Parse(ParseValue(lines[c++]));
				double dist_1_v2s = double.Parse(ParseValue(lines[c++]));

				if (c != lines.Length)
					throw new Exception("FORMAT_ERROR");

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
			}
		}

		private string ParseValue(string line)
		{
			int p = line.IndexOf('=');

			if (p == -1)
				throw new Exception("FORMAT_ERROR");

			line = line.Substring(p + 1);
			p = line.IndexOf(';');

			if (p == -1)
				throw new Exception("FORMAT_ERROR");

			line = line.Substring(0, p);
			line = line.Trim();
			return line;
		}
	}

	// < sync
}
