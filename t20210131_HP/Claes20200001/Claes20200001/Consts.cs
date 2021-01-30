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
		public const string HTTDIR_ROOTDIR = @"C:\app\HTTDir";

		public const string HTTDIR_DOCROOT_LOCAL = "DocRoot.dat";
		public const string HTTDIR_EXE_LOCAL = "HTTDir.exe";

		public static string HTTDIR_DOCROOT_FILE
		{
			get
			{
				return Path.Combine(HTTDIR_ROOTDIR, HTTDIR_DOCROOT_LOCAL);
			}
		}

		public static string HTTDIR_EXE_FILE
		{
			get
			{
				return Path.Combine(HTTDIR_ROOTDIR, HTTDIR_EXE_LOCAL);
			}
		}

		public const string HTTDIR_STOP_EVENT_NAME = "{8bf2b6c0-0a08-48ae-98f3-b875ce5736d6}"; // shared_uuid@ign

		public static string SERVER_PROCESS_MUTEX_NAME
		{
			get
			{
				return ProcMain.APP_IDENT + "_PROCESS_MUTEX";
			}
		}
	}
}
