using System;

namespace client.Models
{
	// Token: 0x02000013 RID: 19
	public class GeoInfo
	{
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000084 RID: 132 RVA: 0x0000283B File Offset: 0x00000A3B
		// (set) Token: 0x06000085 RID: 133 RVA: 0x00002843 File Offset: 0x00000A43
		public string ip { get; set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000086 RID: 134 RVA: 0x0000284C File Offset: 0x00000A4C
		// (set) Token: 0x06000087 RID: 135 RVA: 0x00002854 File Offset: 0x00000A54
		public string city { get; set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000088 RID: 136 RVA: 0x0000285D File Offset: 0x00000A5D
		// (set) Token: 0x06000089 RID: 137 RVA: 0x00002865 File Offset: 0x00000A65
		public string region { get; set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600008A RID: 138 RVA: 0x0000286E File Offset: 0x00000A6E
		// (set) Token: 0x0600008B RID: 139 RVA: 0x00002876 File Offset: 0x00000A76
		public string country { get; set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600008C RID: 140 RVA: 0x0000287F File Offset: 0x00000A7F
		// (set) Token: 0x0600008D RID: 141 RVA: 0x00002887 File Offset: 0x00000A87
		public string country_code { get; set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00002890 File Offset: 0x00000A90
		// (set) Token: 0x0600008F RID: 143 RVA: 0x00002898 File Offset: 0x00000A98
		public string postal { get; set; }

		// Token: 0x06000090 RID: 144 RVA: 0x000028A4 File Offset: 0x00000AA4
		public static GeoInfo ConvertFrom2(GeoInfo2 info2)
		{
			return new GeoInfo
			{
				ip = info2.ip,
				city = info2.city,
				region = info2.region,
				country = info2.country_name,
				country_code = info2.country_code,
				postal = info2.postal
			};
		}
	}
}
