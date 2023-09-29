using System;

namespace client.Decryption
{
	// Token: 0x02000028 RID: 40
	public class ResultStruct
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00004C62 File Offset: 0x00002E62
		// (set) Token: 0x060000FA RID: 250 RVA: 0x00004C6A File Offset: 0x00002E6A
		public string result { get; set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00004C73 File Offset: 0x00002E73
		// (set) Token: 0x060000FC RID: 252 RVA: 0x00004C7B File Offset: 0x00002E7B
		public string id { get; set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00004C84 File Offset: 0x00002E84
		// (set) Token: 0x060000FE RID: 254 RVA: 0x00004C8C File Offset: 0x00002E8C
		public string status_code { get; set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00004C95 File Offset: 0x00002E95
		// (set) Token: 0x06000100 RID: 256 RVA: 0x00004C9D File Offset: 0x00002E9D
		public string api_key { get; set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00004CA6 File Offset: 0x00002EA6
		// (set) Token: 0x06000102 RID: 258 RVA: 0x00004CAE File Offset: 0x00002EAE
		public string response { get; set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00004CB7 File Offset: 0x00002EB7
		// (set) Token: 0x06000104 RID: 260 RVA: 0x00004CBF File Offset: 0x00002EBF
		public string digits { get; set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00004CC8 File Offset: 0x00002EC8
		// (set) Token: 0x06000106 RID: 262 RVA: 0x00004CD0 File Offset: 0x00002ED0
		public string product { get; set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000107 RID: 263 RVA: 0x00004CD9 File Offset: 0x00002ED9
		// (set) Token: 0x06000108 RID: 264 RVA: 0x00004CE1 File Offset: 0x00002EE1
		public string log { get; set; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00004CEA File Offset: 0x00002EEA
		// (set) Token: 0x0600010A RID: 266 RVA: 0x00004CF2 File Offset: 0x00002EF2
		public string errors { get; set; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00004CFB File Offset: 0x00002EFB
		// (set) Token: 0x0600010C RID: 268 RVA: 0x00004D03 File Offset: 0x00002F03
		public string[] hashes { get; set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00004D0C File Offset: 0x00002F0C
		// (set) Token: 0x0600010E RID: 270 RVA: 0x00004D14 File Offset: 0x00002F14
		public string checksum { get; set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00004D1D File Offset: 0x00002F1D
		// (set) Token: 0x06000110 RID: 272 RVA: 0x00004D25 File Offset: 0x00002F25
		public string enabled { get; set; }
	}
}
