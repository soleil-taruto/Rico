using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Charlotte.Commons;
using Charlotte.Tests;
using System.Threading;

namespace Charlotte
{
	class Program
	{
		static void Main(string[] args)
		{
			ProcMain.CUIMain(Main2);
		}

		private static void Main2(ArgsReader ar)
		{
			if (ProcMain.DEBUG)
			{
				TestMain(); // テスト
			}
			else
			{
				ProductMain(ar); // 本番
			}
		}

		private static void TestMain()
		{
			// -- choose one --

			new Test0001().Test01();
			//new Test0001().Test02();
			//new Test0001().Test03();

			// --

			Console.WriteLine("Press ENTER to exit.");
			Console.ReadLine();
		}

		private static void ProductMain(ArgsReader ar)
		{
			if (ar.ArgIs("/E"))
			{
				StopServer();
			}
			else
			{
				StartServer(ar.NextArg());
			}
		}

		private static void StopServer()
		{
			using (EventWaitHandle ev = new EventWaitHandle(false, EventResetMode.AutoReset, Consts.HTTDIR_STOP_EVENT_NAME))
			{
				ev.Set();
			}
		}

		private static void StartServer(string docRootDir)
		{
			// ---- Check ---

			if (!Directory.Exists(Consts.HTTDIR_ROOTDIR))
				throw new Exception("no HTTDIR_ROOTDIR");

			if (!File.Exists(Consts.HTTDIR_EXE_FILE))
				throw new Exception("no HTTDIR_EXE_FILE");

			if (!File.Exists(Consts.HTTDIR_DOCROOT_FILE))
				throw new Exception("no HTTDIR_DOCROOT_FILE");

			docRootDir = SCommon.MakeFullPath(docRootDir);

			if (!Directory.Exists(docRootDir))
				throw new Exception("no docRootDir");

			// ----

			using (Mutex serverProcMutex = new Mutex(false, Consts.SERVER_PROCESS_MUTEX_NAME))
			{
				Console.WriteLine("前回のプロセスの終了を待っています。");

				do
				{
					StopServer();
				}
				while (!serverProcMutex.WaitOne(2000));
				try
				{
					Console.WriteLine("ドキュメントルートを更新します。");
					File.WriteAllLines(Consts.HTTDIR_DOCROOT_FILE, new string[] { "default " + docRootDir }, SCommon.ENCODING_SJIS);

					Console.WriteLine("サーバーを開始します。");
					SCommon.Batch(new string[] { "START /WAIT \"\" \"" + Consts.HTTDIR_EXE_FILE + "\"" });

					Console.WriteLine("サーバーを終了します。");
				}
				finally
				{
					serverProcMutex.ReleaseMutex();
				}
			}
		}
	}
}
