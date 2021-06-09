using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;

namespace Charlotte
{
	public class FolderUpdateMonitor : IDisposable
	{
		private static string SaveDataFile
		{
			get
			{
				return string.Format(@"C:\tmp\FolderUpdateMonitor_{0}.txt", ProcMain.APP_IDENT);
			}
		}

		private class FolderInfo
		{
			public string Dir;
			public string Hash;
		}

		private List<FolderInfo> Folders;

		public FolderUpdateMonitor()
		{
			if (File.Exists(SaveDataFile))
			{
				this.Folders = File.ReadAllLines(SaveDataFile, Encoding.UTF8).Select(line =>
				{
					string[] tokens = line.Split('\t');

					return new FolderInfo
					{
						Dir = tokens[0],
						Hash = tokens[1],
					};
				})
				.ToList();

				this.Folders.RemoveAll(folder => !Directory.Exists(folder.Dir));
			}
			else
			{
				this.Folders = new List<FolderInfo>();
			}
		}

		public bool IsUpdated(string dir)
		{
			FolderInfo folder = this.Folders.FirstOrDefault(v => SCommon.EqualsIgnoreCase(v.Dir, dir));
			string hash = GetHash(dir);

			if (folder == null)
			{
				this.Folders.Add(new FolderInfo()
				{
					Dir = dir,
					Hash = hash,
				});

				return true;
			}
			if (folder.Hash != hash)
			{
				folder.Hash = hash;
				return true;
			}
			return false;
		}

		private static string GetHash(string rootDir)
		{
			StringBuilder buff = new StringBuilder();

			foreach (string dir in Directory.GetDirectories(rootDir, "*", SearchOption.AllDirectories).OrderBy(SCommon.Comp))
			{
				buff.Append('D');
				buff.Append(dir);
				buff.Append('*');
			}

			foreach (string file in Directory.GetFiles(rootDir, "*", SearchOption.AllDirectories).OrderBy(SCommon.Comp))
			{
				FileInfo fileInfo = new FileInfo(file);

				buff.Append('F');
				buff.Append(file);
				buff.Append('*');
				buff.Append(fileInfo.CreationTime);
				buff.Append('*');
				buff.Append(fileInfo.LastWriteTime);
				buff.Append('*');
			}

			string str = buff.ToString();
			byte[] bStr = Encoding.UTF8.GetBytes(str);
			byte[] bHash_512 = SCommon.GetSHA512(bStr);
			byte[] bHash_128 = SCommon.GetSubBytes(bHash_512, 0, 16);
			string hash_128 = SCommon.Hex.ToString(bHash_128);
			return hash_128;
		}

		public void Dispose()
		{
			if (this.Folders != null)
			{
				File.WriteAllLines(SaveDataFile, this.Folders.Select(folder => folder.Dir + "\t" + folder.Hash), Encoding.UTF8);
				this.Folders = null;
			}
		}
	}
}
