using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace client.Helpers
{
	// Token: 0x0200001A RID: 26
	internal class JsonHelper
	{
		// Token: 0x060000AD RID: 173 RVA: 0x00002BC0 File Offset: 0x00000DC0
		public static T Deserialize<T>(string json)
		{
			T result;
			using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
			{
				result = (T)((object)new DataContractJsonSerializer(typeof(T)).ReadObject(memoryStream));
			}
			return result;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00002C18 File Offset: 0x00000E18
		public static string Serialize<T>(T instance)
		{
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));
			string @string;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				dataContractJsonSerializer.WriteObject(memoryStream, instance);
				@string = Encoding.UTF8.GetString(memoryStream.ToArray());
			}
			return @string;
		}
	}
}
