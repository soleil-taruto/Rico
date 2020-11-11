using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Charlotte.Tests
{
	public class Test0001
	{
		public void Test01()
		{
			// memo: CSSolution.Confuse() はソリューション配下のソースファイルを書き換えるので、安全のため ElsaConfuser.Perform() 経由でテストすること。

			//ElsaConfuser.Perform(@"C:\Dev\Elsa\e20200928_NovelAdv\Elsa20200001\Elsa20200001.sln", @"C:\temp");
			//ElsaConfuser.Perform(@"C:\Dev\Elsa\e20201008_NovelAdv_Demo\Elsa20200001\Elsa20200001.sln", @"C:\temp");
			//ElsaConfuser.Perform(@"C:\Dev\Elsa\e20201018_TateShoot_Demo\Elsa20200001\Elsa20200001.sln", @"C:\temp");
			ElsaConfuser.Perform(@"C:\Dev\Elsa\e20201027_YokoActTM_Demo\Elsa20200001\Elsa20200001.sln", @"C:\temp");
		}
	}
}
