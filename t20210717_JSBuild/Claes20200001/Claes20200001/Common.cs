using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using Microsoft.VisualBasic;
using Charlotte.Commons;

namespace Charlotte
{
	public static class Common
	{
		public static void Pause()
		{
			Console.WriteLine("Press ENTER key.");
			Console.ReadLine();
		}

		#region GetOutputDir

		private static string GOD_Dir;

		public static string GetOutputDir()
		{
			if (GOD_Dir == null)
				GOD_Dir = GetOutputDir_Main();

			return GOD_Dir;
		}

		private static string GetOutputDir_Main()
		{
			for (int c = 1; c <= 999; c++)
			{
				string dir = "C:\\" + c;

				if (
					!Directory.Exists(dir) &&
					!File.Exists(dir)
					)
				{
					SCommon.CreateDir(dir);
					//SCommon.Batch(new string[] { "START " + dir });
					return dir;
				}
			}
			throw new Exception("C:\\1 ～ 999 は使用できません。");
		}

		public static void OpenOutputDir()
		{
			SCommon.Batch(new string[] { "START " + GetOutputDir() });
		}

		public static void OpenOutputDirIfCreated()
		{
			if (GOD_Dir != null)
			{
				OpenOutputDir();
			}
		}

		private static int NOP_Count = 0;

		public static string NextOutputPath()
		{
			return Path.Combine(GetOutputDir(), (++NOP_Count).ToString("D4"));
		}

		#endregion

		public static double GetDistance(D2Point pt)
		{
			return Math.Sqrt(pt.X * pt.X + pt.Y * pt.Y);
		}

		public static string[] ParseIsland(string text, string singleTag, bool ignoreCase = false)
		{
			int start;

			if (ignoreCase)
				start = text.ToLower().IndexOf(singleTag.ToLower());
			else
				start = text.IndexOf(singleTag);

			if (start == -1)
				return null;

			int end = start + singleTag.Length;

			return new string[]
			{
				text.Substring(0, start),
				text.Substring(start, end - start),
				text.Substring(end),
			};
		}

		public static string[] ParseEnclosed(string text, string openTag, string closeTag, bool ignoreCase = false)
		{
			string[] starts = ParseIsland(text, openTag, ignoreCase);

			if (starts == null)
				return null;

			string[] ends = ParseIsland(starts[2], closeTag, ignoreCase);

			if (ends == null)
				return null;

			return new string[]
			{
				starts[0],
				starts[1],
				ends[0],
				ends[1],
				ends[2],
			};
		}

		#region ToFairLocalPath

		/// <summary>
		/// Windowsのローカル名に使用出来ない予約名のリストを返す。
		/// 今では使用可能なものも含む。
		/// 元にしたコード：
		/// https://github.com/stackprobe/Factory/blob/master/Common/DataConv.c#L460-L491
		/// </summary>
		/// <returns>予約名リスト</returns>
		private static IEnumerable<string> GetReservedWordsForWindowsPath()
		{
			yield return "AUX";
			yield return "CON";
			yield return "NUL";
			yield return "PRN";

			for (int no = 1; no <= 9; no++)
			{
				yield return "COM" + no;
				yield return "LPT" + no;
			}

			// グレーゾーン
			{
				yield return "COM0";
				yield return "LPT0";
				yield return "CLOCK$";
				yield return "CONFIG$";
			}
		}

		/// <summary>
		/// 歴としたローカル名に変換する。
		/// 実際に使用可能なローカル名より基準が厳しい。
		/// 元にしたコード：
		/// https://github.com/stackprobe/Factory/blob/master/Common/DataConv.c#L503-L552
		/// </summary>
		/// <param name="str">対象文字列(対象パス)</param>
		/// <param name="dirSize">対象パスが存在するディレクトリのフルパスの長さ、考慮しない場合は 0 を指定すること。</param>
		/// <returns>ローカル名</returns>
		public static string ToFairLocalPath(string str, int dirSize)
		{
			const int MY_PATH_MAX = 250;
			const string NG_CHARS = "\"*/:<>?\\|";
			const string ALT_WORD = "_";

			int localPathSizeMax = Math.Max(0, MY_PATH_MAX - dirSize);

			if (localPathSizeMax < str.Length)
				str = str.Substring(0, localPathSizeMax);

			str = SCommon.ToJString(SCommon.ENCODING_SJIS.GetBytes(str), true, false, false, true);

			string[] words = str.Split('.');

			for (int index = 0; index < words.Length; index++)
			{
				string word = words[index];

				word = word.Trim();

				if (
					index == 0 &&
					GetReservedWordsForWindowsPath().Any(resWord => SCommon.EqualsIgnoreCase(resWord, word)) ||
					word.Any(chr => NG_CHARS.IndexOf(chr) != -1)
					)
					word = ALT_WORD;

				words[index] = word;
			}
			str = string.Join(".", words);

			if (str == "")
				str = ALT_WORD;

			if (str.EndsWith("."))
				str = str.Substring(0, str.Length - 1) + ALT_WORD;

			return str;
		}

		#endregion

		/// <summary>
		/// 歴としたローカル名か判定する。
		/// 実際に使用可能なローカル名より基準が厳しい。
		/// </summary>
		/// <param name="localPath">対象パス</param>
		/// <param name="dirSize">対象パスが存在するディレクトリのフルパスの長さ、考慮しない場合は 0 を指定すること。</param>
		/// <returns>ローカル名か</returns>
		public static bool IsFairLocalPath(string localPath, int dirSize = 0)
		{
			return ToFairLocalPath(localPath, dirSize) == localPath;
		}

		/// <summary>
		/// 歴とした(ハイパーリンクの)相対パス名か判定する。
		/// </summary>
		/// <param name="path">対象パス</param>
		/// <returns>相対パス名か</returns>
		public static bool IsFairHrefRelPath(string path)
		{
			return !path.Split('/').Any(ptkn => ptkn != "." && ptkn != ".." && !IsFairLocalPath(ptkn));
		}

		#region Ext2MediaTypePairs

		/// <summary>
		/// https://github.com/stackprobe/HTT/blob/master/doc/MimeType.tsv
		/// </summary>
		private static string[] Ext2MediaTypePairs = new string[]
		{
			".323",     "text/h323",
			".acx",     "application/internet-property-stream",
			".ai",      "application/postscript",
			".aif",     "audio/x-aiff",
			".aifc",    "audio/x-aiff",
			".aiff",    "audio/x-aiff",
			".asf",     "video/x-ms-asf",
			".asr",     "video/x-ms-asf",
			".asx",     "video/x-ms-asf",
			".au",      "audio/basic",
			".avi",     "video/x-msvideo",
			".axs",     "application/olescript",
			".bas",     "text/plain",
			".bcpio",   "application/x-bcpio",
			".bin",     "application/octet-stream",
			".bmp",     "image/bmp",
			".c",       "text/plain",
			".cat",     "application/vnd.ms-pkiseccat",
			".cdf",     "application/x-cdf",
			".cer",     "application/x-x509-ca-cert",
			".class",   "application/octet-stream",
			".clp",     "application/x-msclip",
			".cmx",     "image/x-cmx",
			".cod",     "image/cis-cod",
			".cpio",    "application/x-cpio",
			".crd",     "application/x-mscardfile",
			".crl",     "application/pkix-crl",
			".crt",     "application/x-x509-ca-cert",
			".csh",     "application/x-csh",
			".css",     "text/css",
			".dcr",     "application/x-director",
			".der",     "application/x-x509-ca-cert",
			".dir",     "application/x-director",
			".dll",     "application/x-msdownload",
			".dms",     "application/octet-stream",
			".doc",     "application/msword",
			".dot",     "application/msword",
			".dvi",     "application/x-dvi",
			".dxr",     "application/x-director",
			".eps",     "application/postscript",
			".etx",     "text/x-setext",
			".evy",     "application/envoy",
			".exe",     "application/octet-stream",
			".fif",     "application/fractals",
			".flr",     "x-world/x-vrml",
			".gif",     "image/gif",
			".gtar",    "application/x-gtar",
			".gz",      "application/x-gzip",
			".h",       "text/plain",
			".hdf",     "application/x-hdf",
			".hlp",     "application/winhlp",
			".hqx",     "application/mac-binhex40",
			".hta",     "application/hta",
			".htc",     "text/x-component",
			".htm",     "text/html",
			".html",    "text/html",
			".htt",     "text/webviewhtml",
			".ico",     "image/x-icon",
			".ief",     "image/ief",
			".iii",     "application/x-iphone",
			".ins",     "application/x-internet-signup",
			".isp",     "application/x-internet-signup",
			".jfif",    "image/pipeg",
			".jpe",     "image/jpeg",
			".jpeg",    "image/jpeg",
			".jpg",     "image/jpeg",
			".js",      "application/x-javascript",
			".json",    "application/json",
			".latex",   "application/x-latex",
			".lha",     "application/octet-stream",
			".lsf",     "video/x-la-asf",
			".lsx",     "video/x-la-asf",
			".lzh",     "application/octet-stream",
			".m13",     "application/x-msmediaview",
			".m14",     "application/x-msmediaview",
			".m3u",     "audio/x-mpegurl",
			".m4a",     "audio/aac",
			".m4v",     "video/mp4",
			".man",     "application/x-troff-man",
			".mdb",     "application/x-msaccess",
			".me",      "application/x-troff-me",
			".mht",     "message/rfc822",
			".mhtml",   "message/rfc822",
			".mid",     "audio/midi",
			".midi",    "audio/midi",
			".mny",     "application/x-msmoney",
			".mov",     "video/quicktime",
			".movie",   "video/x-sgi-movie",
			".mp2",     "video/mpeg",
			".mp3",     "audio/mpeg",
			".mp4",     "video/mp4",
			".mpa",     "video/mpeg",
			".mpe",     "video/mpeg",
			".mpeg",    "video/mpeg",
			".mpg",     "video/mpeg",
			".mpga",    "audio/mpeg",
			".mpp",     "application/vnd.ms-project",
			".mpv2",    "video/mpeg",
			".ms",      "application/x-troff-ms",
			".mvb",     "application/x-msmediaview",
			".nws",     "message/rfc822",
			".oda",     "application/oda",
			".ogv",     "video/ogg",
			".p10",     "application/pkcs10",
			".p12",     "application/x-pkcs12",
			".p7b",     "application/x-pkcs7-certificates",
			".p7c",     "application/x-pkcs7-mime",
			".p7m",     "application/x-pkcs7-mime",
			".p7r",     "application/x-pkcs7-certreqresp",
			".p7s",     "application/x-pkcs7-signature",
			".pbm",     "image/x-portable-bitmap",
			".pdf",     "application/pdf",
			".pfx",     "application/x-pkcs12",
			".pgm",     "image/x-portable-graymap",
			".pko",     "application/ynd.ms-pkipko",
			".pma",     "application/x-perfmon",
			".pmc",     "application/x-perfmon",
			".pml",     "application/x-perfmon",
			".pmr",     "application/x-perfmon",
			".pmw",     "application/x-perfmon",
			".png",     "image/png",
			".pnm",     "image/x-portable-anymap",
			".pot",     "application/vnd.ms-powerpoint",
			".ppm",     "image/x-portable-pixmap",
			".pps",     "application/vnd.ms-powerpoint",
			".ppt",     "application/vnd.ms-powerpoint",
			".prf",     "application/pics-rules",
			".ps",      "application/postscript",
			".pub",     "application/x-mspublisher",
			".qt",      "video/quicktime",
			".ra",      "audio/x-pn-realaudio",
			".ram",     "audio/x-pn-realaudio",
			".ras",     "image/x-cmu-raster",
			".rgb",     "image/x-rgb",
			".rmi",     "audio/mid",
			".roff",    "application/x-troff",
			".rtf",     "application/rtf",
			".rtx",     "text/richtext",
			".scd",     "application/x-msschedule",
			".sct",     "text/scriptlet",
			".setpay",  "application/set-payment-initiation",
			".setreg",  "application/set-registration-initiation",
			".sh",      "application/x-sh",
			".shar",    "application/x-shar",
			".sit",     "application/x-stuffit",
			".snd",     "audio/basic",
			".spc",     "application/x-pkcs7-certificates",
			".spl",     "application/futuresplash",
			".src",     "application/x-wais-source",
			".sst",     "application/vnd.ms-pkicertstore",
			".stl",     "application/vnd.ms-pkistl",
			".stm",     "text/html",
			".sv4cpio", "application/x-sv4cpio",
			".sv4crc",  "application/x-sv4crc",
			".svg",     "image/svg+xml",
			".swf",     "application/x-shockwave-flash",
			".t",       "application/x-troff",
			".tar",     "application/x-tar",
			".tcl",     "application/x-tcl",
			".tex",     "application/x-tex",
			".texi",    "application/x-texinfo",
			".texinfo", "application/x-texinfo",
			".tgz",     "application/x-compressed",
			".tif",     "image/tiff",
			".tiff",    "image/tiff",
			".tr",      "application/x-troff",
			".trm",     "application/x-msterminal",
			".tsv",     "text/tab-separated-values",
			".txt",     "text/plain",
			".uls",     "text/iuls",
			".ustar",   "application/x-ustar",
			".vcf",     "text/x-vcard",
			".vrml",    "x-world/x-vrml",
			".wav",     "audio/x-wav",
			".wcm",     "application/vnd.ms-works",
			".wdb",     "application/vnd.ms-works",
			".webm",    "video/webm",
			".wks",     "application/vnd.ms-works",
			".wmf",     "application/x-msmetafile",
			".wps",     "application/vnd.ms-works",
			".wri",     "application/x-mswrite",
			".wrl",     "x-world/x-vrml",
			".wrz",     "x-world/x-vrml",
			".xaf",     "x-world/x-vrml",
			".xbm",     "image/x-xbitmap",
			".xht",     "application/xhtml+xml",
			".xhtml",   "application/xhtml+xml",
			".xla",     "application/vnd.ms-excel",
			".xlc",     "application/vnd.ms-excel",
			".xlm",     "application/vnd.ms-excel",
			".xls",     "application/vnd.ms-excel",
			".xlt",     "application/vnd.ms-excel",
			".xlw",     "application/vnd.ms-excel",
			".xml",     "text/xml",
			".xof",     "x-world/x-vrml",
			".xpm",     "image/x-xpixmap",
			".xsl",     "text/xml",
			".xwd",     "image/x-xwindowdump",
			".z",       "application/x-compress",
			".zip",     "application/zip",
		};

		#endregion

		public static string GetMediaTypeByExt(string ext)
		{
			ext = ext.ToLower();

			for (int index = 0; index < Ext2MediaTypePairs.Length; index += 2)
				if (Ext2MediaTypePairs[index] == ext)
					return Ext2MediaTypePairs[index + 1];

			return "application/octet-stream";
		}

		public static string MakeFullPathByBaseDir(string file, string baseDir)
		{
			string homeDir = Directory.GetCurrentDirectory();
			Directory.SetCurrentDirectory(baseDir);
			try
			{
				file = SCommon.MakeFullPath(file);
			}
			finally
			{
				try
				{
					Directory.SetCurrentDirectory(homeDir);
				}
				catch (Exception e)
				{
					ProcMain.WriteLog(e);
				}
			}
			return file;
		}

		private static string ConvPathForCompPath(string path)
		{
			string[] ptkns = path.Split('\\');

			for (int index = 0; index < ptkns.Length; index++)
			{
				string prefix;

				if (index + 1 < path.Length) // ? ディレクトリ
					prefix = "1";
				else // ? ファイル
					prefix = "2";

				ptkns[index] = prefix + ptkns[index];
			}
			path = string.Join("\t", ptkns);
			path = path.Replace('.', '\n');
			path = path.ToLower();
			return path;
		}

		public static int CompPath(string path1, string path2)
		{
			path1 = ConvPathForCompPath(path1);
			path2 = ConvPathForCompPath(path2);

			return SCommon.Comp(path1, path2);
		}
	}
}
