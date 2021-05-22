using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0001
	{
		public void Test01()
		{
			VoyagerDistance vd = new VoyagerDistance();

			Print(vd.Earth_Voyager_1, "Earth to Voyager 1");
			Print(vd.Earth_Voyager_2, "Earth to Voyager 2");
			Print(vd.Sun_Voyager_1, "Sun to Voyager 1");
			Print(vd.Sun_Voyager_2, "Sun to Voyager 2");
		}

		private void Print(VoyagerDistance.DistancePairInfo pair, string prompt)
		{
			Print(pair.A, prompt);
			Print(pair.B, prompt);

			SCommon.SimpleDateTime now = SCommon.SimpleDateTime.Now();

			Console.WriteLine(prompt + " --> " + now + ", " + pair.GetKm(now) + " KM (NOW)");
		}

		private void Print(VoyagerDistance.DistanceInfo info, string prompt)
		{
			Console.WriteLine(prompt + " --> " + info.DateTime + ", " + info.Km + " KM");
		}
	}
}
