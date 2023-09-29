using System;
using System.Collections.Generic;

namespace client.Models
{
	// Token: 0x02000010 RID: 16
	public class GeckoBrowserStructure
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000069 RID: 105 RVA: 0x0000276F File Offset: 0x0000096F
		// (set) Token: 0x0600006A RID: 106 RVA: 0x00002777 File Offset: 0x00000977
		public string GeckoBrowserName { get; set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00002780 File Offset: 0x00000980
		// (set) Token: 0x0600006C RID: 108 RVA: 0x00002788 File Offset: 0x00000988
		public List<GeckoDefaultProfile> GeckoProfiles { get; set; }
	}
}
