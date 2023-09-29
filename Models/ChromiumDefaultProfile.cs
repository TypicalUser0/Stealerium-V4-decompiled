using System;
using System.Collections.Generic;

namespace client.Models
{
	// Token: 0x02000012 RID: 18
	public class ChromiumDefaultProfile
	{
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000079 RID: 121 RVA: 0x000027E6 File Offset: 0x000009E6
		// (set) Token: 0x0600007A RID: 122 RVA: 0x000027EE File Offset: 0x000009EE
		public string ChromiumProfileName { get; set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600007B RID: 123 RVA: 0x000027F7 File Offset: 0x000009F7
		// (set) Token: 0x0600007C RID: 124 RVA: 0x000027FF File Offset: 0x000009FF
		public string ChromiumLoginData { get; set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00002808 File Offset: 0x00000A08
		// (set) Token: 0x0600007E RID: 126 RVA: 0x00002810 File Offset: 0x00000A10
		public string ChromiumCookies { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00002819 File Offset: 0x00000A19
		// (set) Token: 0x06000080 RID: 128 RVA: 0x00002821 File Offset: 0x00000A21
		public string ChromiumWebData { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000081 RID: 129 RVA: 0x0000282A File Offset: 0x00000A2A
		// (set) Token: 0x06000082 RID: 130 RVA: 0x00002832 File Offset: 0x00000A32
		public List<DefaultFileStructure> ChromiumExtensions { get; set; }
	}
}
