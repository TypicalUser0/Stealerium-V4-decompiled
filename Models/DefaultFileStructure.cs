using System;

namespace client.Models
{
	// Token: 0x0200000E RID: 14
	public class DefaultFileStructure
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00002709 File Offset: 0x00000909
		// (set) Token: 0x0600005C RID: 92 RVA: 0x00002711 File Offset: 0x00000911
		public DataType Service { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600005D RID: 93 RVA: 0x0000271A File Offset: 0x0000091A
		// (set) Token: 0x0600005E RID: 94 RVA: 0x00002722 File Offset: 0x00000922
		public string FileName { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600005F RID: 95 RVA: 0x0000272B File Offset: 0x0000092B
		// (set) Token: 0x06000060 RID: 96 RVA: 0x00002733 File Offset: 0x00000933
		public byte[] FileContent { get; set; }
	}
}
