using System;
using System.IO;
using System.Net;
using System.Text;
using client.Helpers;

namespace client.Network
{
	// Token: 0x02000007 RID: 7
	internal class ErrorReporter
	{
		// Token: 0x06000011 RID: 17 RVA: 0x000023F4 File Offset: 0x000005F4
		public static void Report(Exception ex)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://*Ваш домен*/error_report");
			httpWebRequest.ContentType = "application/json";
			httpWebRequest.Method = "POST";
			using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
			{
				string value = JsonHelper.Serialize<ErrorStruct>(new ErrorStruct
				{
					dead_inside = Config.UserID + " reported error: " + ErrorReporter.FlattenException(ex)
				});
				streamWriter.Write(value);
			}
			new StreamReader(((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream()).ReadToEnd();
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002498 File Offset: 0x00000698
		public static string FlattenException(Exception exception)
		{
			StringBuilder stringBuilder = new StringBuilder();
			while (exception != null)
			{
				stringBuilder.AppendLine(exception.Message);
				stringBuilder.AppendLine(exception.StackTrace);
				exception = exception.InnerException;
			}
			return stringBuilder.ToString();
		}
	}
}
