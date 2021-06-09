﻿using System;
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
			Test01_a(@"C:\Factory", "Factory");
			Test01_a(@"C:\Dev\Elsa3", "Elsa3");
			Test01_a(@"C:\Dev\wb2", "wb2");
		}

		private void Test01_a(string rDir, string wLocalDir)
		{
			string wDir = Path.Combine(@"C:\temp\CopyToRepository_Test", wLocalDir);

			//SCommon.DeletePath(wDir);
			SCommon.CreateDir(wDir);

			new Program().CopyToRepository(rDir, wDir);
		}
	}
}
