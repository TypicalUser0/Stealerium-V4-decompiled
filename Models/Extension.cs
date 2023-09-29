using System;
using System.Collections.Generic;

namespace client.Models
{
	// Token: 0x0200000B RID: 11
	public class Extension
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002582 File Offset: 0x00000782
		// (set) Token: 0x0600002B RID: 43 RVA: 0x0000258A File Offset: 0x0000078A
		public string Path { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00002593 File Offset: 0x00000793
		// (set) Token: 0x0600002D RID: 45 RVA: 0x0000259B File Offset: 0x0000079B
		public string FileName { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600002E RID: 46 RVA: 0x000025A4 File Offset: 0x000007A4
		// (set) Token: 0x0600002F RID: 47 RVA: 0x000025AC File Offset: 0x000007AC
		public List<string> Data { get; set; }
	}
}
