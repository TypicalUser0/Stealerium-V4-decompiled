using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace client.Decryption
{
	// Token: 0x02000034 RID: 52
	internal class BerkeleyDatabase
	{
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600013F RID: 319 RVA: 0x0000601C File Offset: 0x0000421C
		// (set) Token: 0x06000140 RID: 320 RVA: 0x00006024 File Offset: 0x00004224
		public string Version { get; set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000141 RID: 321 RVA: 0x0000602D File Offset: 0x0000422D
		// (set) Token: 0x06000142 RID: 322 RVA: 0x00006035 File Offset: 0x00004235
		public List<KeyValuePair<string, string>> Keys { get; private set; }

		// Token: 0x06000143 RID: 323 RVA: 0x00006040 File Offset: 0x00004240
		public BerkeleyDatabase(byte[] file)
		{
			List<byte> list = new List<byte>();
			this.Keys = new List<KeyValuePair<string, string>>();
			using (Stream stream = new MemoryStream(file))
			{
				using (BinaryReader binaryReader = new BinaryReader(stream))
				{
					int i = 0;
					int num = (int)binaryReader.BaseStream.Length;
					while (i < num)
					{
						list.Add(binaryReader.ReadByte());
						i++;
					}
				}
			}
			string text = BitConverter.ToString(this.Extract(list.ToArray(), 0, 4, false)).Replace("-", "");
			string text2 = BitConverter.ToString(this.Extract(list.ToArray(), 4, 4, false)).Replace("-", "");
			int num2 = BitConverter.ToInt32(this.Extract(list.ToArray(), 12, 4, true), 0);
			if (text.Equals("00061561"))
			{
				this.Version = "Berkelet DB";
				if (text2.Equals("00000002"))
				{
					this.Version += " 1.85 (Hash, version 2, native byte-order)";
				}
				int num3 = int.Parse(BitConverter.ToString(this.Extract(list.ToArray(), 56, 4, false)).Replace("-", ""));
				int num4 = 1;
				while (this.Keys.Count < num3)
				{
					string[] array = new string[(num3 - this.Keys.Count) * 2];
					for (int j = 0; j < (num3 - this.Keys.Count) * 2; j++)
					{
						array[j] = BitConverter.ToString(this.Extract(list.ToArray(), num2 * num4 + 2 + j * 2, 2, true)).Replace("-", "");
					}
					Array.Sort<string>(array);
					for (int k = 0; k < array.Length; k += 2)
					{
						int num5 = Convert.ToInt32(array[k], 16) + num2 * num4;
						int num6 = Convert.ToInt32(array[k + 1], 16) + num2 * num4;
						int num7 = (k + 2 >= array.Length) ? (num2 + num2 * num4) : (Convert.ToInt32(array[k + 2], 16) + num2 * num4);
						string @string = Encoding.ASCII.GetString(this.Extract(list.ToArray(), num6, num7 - num6, false));
						string value = BitConverter.ToString(this.Extract(list.ToArray(), num5, num6 - num5, false));
						if (!string.IsNullOrWhiteSpace(@string))
						{
							this.Keys.Add(new KeyValuePair<string, string>(@string, value));
						}
					}
					num4++;
				}
				return;
			}
			this.Version = "Unknow database format";
		}

		// Token: 0x06000144 RID: 324 RVA: 0x000062F8 File Offset: 0x000044F8
		private byte[] Extract(byte[] source, int start, int length, bool littleEndian)
		{
			byte[] array = new byte[length];
			int num = 0;
			for (int i = start; i < start + length; i++)
			{
				array[num] = source[i];
				num++;
			}
			if (littleEndian)
			{
				Array.Reverse(array);
			}
			return array;
		}
	}
}
