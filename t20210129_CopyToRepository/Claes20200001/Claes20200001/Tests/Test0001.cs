using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0001
	{
		public void Test01()
		{
			Test01_a(@"C:\Factory", @"C:\temp\Test0001_Factory");
			Test01_a(@"C:\Dev\wb", @"C:\temp\Test0001_wb");
		}

		private void Test01_a(string rDir, string wDir)
		{
			SCommon.DeletePath(wDir);
			SCommon.CreateDir(wDir);

			new Program().CopyToRepository(rDir, wDir);
		}
	}
}
