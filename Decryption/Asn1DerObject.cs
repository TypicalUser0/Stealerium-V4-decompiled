using System;
using System.Collections.Generic;
using System.Text;

namespace client.Decryption
{
	// Token: 0x02000033 RID: 51
	public class Asn1DerObject
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00005DF5 File Offset: 0x00003FF5
		// (set) Token: 0x06000136 RID: 310 RVA: 0x00005DFD File Offset: 0x00003FFD
		public Asn1Der.Type Type { get; set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00005E06 File Offset: 0x00004006
		// (set) Token: 0x06000138 RID: 312 RVA: 0x00005E0E File Offset: 0x0000400E
		public int Lenght { get; set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00005E17 File Offset: 0x00004017
		// (set) Token: 0x0600013A RID: 314 RVA: 0x00005E1F File Offset: 0x0000401F
		public List<Asn1DerObject> objects { get; set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600013B RID: 315 RVA: 0x00005E28 File Offset: 0x00004028
		// (set) Token: 0x0600013C RID: 316 RVA: 0x00005E30 File Offset: 0x00004030
		public byte[] Data { get; set; }

		// Token: 0x0600013D RID: 317 RVA: 0x00005E39 File Offset: 0x00004039
		public Asn1DerObject()
		{
			this.objects = new List<Asn1DerObject>();
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00005E4C File Offset: 0x0000404C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			Asn1Der.Type type = this.Type;
			switch (type)
			{
			case Asn1Der.Type.Integer:
			{
				foreach (byte b in this.Data)
				{
					stringBuilder2.AppendFormat("{0:X2}", b);
				}
				StringBuilder stringBuilder3 = stringBuilder;
				string str = "\tINTEGER ";
				StringBuilder stringBuilder4 = stringBuilder2;
				stringBuilder3.AppendLine(str + ((stringBuilder4 != null) ? stringBuilder4.ToString() : null));
				stringBuilder2 = new StringBuilder();
				break;
			}
			case Asn1Der.Type.BitString:
			case Asn1Der.Type.Null:
				break;
			case Asn1Der.Type.OctetString:
				foreach (byte b2 in this.Data)
				{
					stringBuilder2.AppendFormat("{0:X2}", b2);
				}
				stringBuilder.AppendLine("\tOCTETSTRING " + stringBuilder2.ToString());
				stringBuilder2 = new StringBuilder();
				break;
			case Asn1Der.Type.ObjectIdentifier:
				foreach (byte b3 in this.Data)
				{
					stringBuilder2.AppendFormat("{0:X2}", b3);
				}
				stringBuilder.AppendLine("\tOBJECTIDENTIFIER " + stringBuilder2.ToString());
				stringBuilder2 = new StringBuilder();
				break;
			default:
				if (type == Asn1Der.Type.Sequence)
				{
					stringBuilder.AppendLine("SEQUENCE {");
				}
				break;
			}
			foreach (Asn1DerObject asn1DerObject in this.objects)
			{
				stringBuilder.Append(asn1DerObject.ToString());
			}
			if (this.Type.Equals(Asn1Der.Type.Sequence))
			{
				stringBuilder.AppendLine("}");
			}
			return stringBuilder.ToString();
		}
	}
}
