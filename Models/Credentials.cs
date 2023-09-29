using System;

namespace client.Models
{
	// Token: 0x0200000C RID: 12
	public class Credentials
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000031 RID: 49 RVA: 0x000025B5 File Offset: 0x000007B5
		// (set) Token: 0x06000032 RID: 50 RVA: 0x000025BD File Offset: 0x000007BD
		public string CredentialName { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000033 RID: 51 RVA: 0x000025C6 File Offset: 0x000007C6
		// (set) Token: 0x06000034 RID: 52 RVA: 0x000025CE File Offset: 0x000007CE
		public string CredentialHostname { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000035 RID: 53 RVA: 0x000025D7 File Offset: 0x000007D7
		// (set) Token: 0x06000036 RID: 54 RVA: 0x000025DF File Offset: 0x000007DF
		public string CredentialUsername { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000037 RID: 55 RVA: 0x000025E8 File Offset: 0x000007E8
		// (set) Token: 0x06000038 RID: 56 RVA: 0x000025F0 File Offset: 0x000007F0
		public string CredentialPassword { get; set; }
	}
}
