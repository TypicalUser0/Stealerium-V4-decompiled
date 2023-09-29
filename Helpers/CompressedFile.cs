using System;

namespace client.Helpers
{
	// Token: 0x02000024 RID: 36
	public class CompressedFile
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x0000357C File Offset: 0x0000177C
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x00003584 File Offset: 0x00001784
		public string Name { get; set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x0000358D File Offset: 0x0000178D
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x00003595 File Offset: 0x00001795
		public byte[] Content { get; set; }
	}
}
