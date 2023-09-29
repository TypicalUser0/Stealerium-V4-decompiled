using System;
using System.Collections.Generic;

namespace client.Models
{
	// Token: 0x0200000F RID: 15
	public class ChromiumBrowserStructure
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000062 RID: 98 RVA: 0x0000273C File Offset: 0x0000093C
		// (set) Token: 0x06000063 RID: 99 RVA: 0x00002744 File Offset: 0x00000944
		public byte[] PrivateKey { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000064 RID: 100 RVA: 0x0000274D File Offset: 0x0000094D
		// (set) Token: 0x06000065 RID: 101 RVA: 0x00002755 File Offset: 0x00000955
		public string ChromiumBrowserName { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000066 RID: 102 RVA: 0x0000275E File Offset: 0x0000095E
		// (set) Token: 0x06000067 RID: 103 RVA: 0x00002766 File Offset: 0x00000966
		public List<ChromiumDefaultProfile> ChromiumProfiles { get; set; }
	}
}
