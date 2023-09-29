using System;

namespace client.Decryption
{
	// Token: 0x02000031 RID: 49
	public class Asn1Der
	{
		// Token: 0x06000133 RID: 307 RVA: 0x00005B38 File Offset: 0x00003D38
		public Asn1DerObject Parse(byte[] toparse)
		{
			Asn1DerObject asn1DerObject = new Asn1DerObject();
			for (int i = 0; i < toparse.Length; i++)
			{
				Asn1Der.Type type = (Asn1Der.Type)toparse[i];
				switch (type)
				{
				case Asn1Der.Type.Integer:
				{
					asn1DerObject.objects.Add(new Asn1DerObject
					{
						Type = Asn1Der.Type.Integer,
						Lenght = (int)toparse[i + 1]
					});
					byte[] array = new byte[(int)toparse[i + 1]];
					int length = (i + 2 + (int)toparse[i + 1] > toparse.Length) ? (toparse.Length - (i + 2)) : ((int)toparse[i + 1]);
					Array.Copy(toparse, i + 2, array, 0, length);
					Asn1DerObject[] array2 = asn1DerObject.objects.ToArray();
					asn1DerObject.objects[array2.Length - 1].Data = array;
					i = i + 1 + asn1DerObject.objects[array2.Length - 1].Lenght;
					break;
				}
				case Asn1Der.Type.BitString:
				case Asn1Der.Type.Null:
					break;
				case Asn1Der.Type.OctetString:
				{
					asn1DerObject.objects.Add(new Asn1DerObject
					{
						Type = Asn1Der.Type.OctetString,
						Lenght = (int)toparse[i + 1]
					});
					byte[] array = new byte[(int)toparse[i + 1]];
					int length = (i + 2 + (int)toparse[i + 1] > toparse.Length) ? (toparse.Length - (i + 2)) : ((int)toparse[i + 1]);
					Array.Copy(toparse, i + 2, array, 0, length);
					Asn1DerObject[] array3 = asn1DerObject.objects.ToArray();
					asn1DerObject.objects[array3.Length - 1].Data = array;
					i = i + 1 + asn1DerObject.objects[array3.Length - 1].Lenght;
					break;
				}
				case Asn1Der.Type.ObjectIdentifier:
				{
					asn1DerObject.objects.Add(new Asn1DerObject
					{
						Type = Asn1Der.Type.ObjectIdentifier,
						Lenght = (int)toparse[i + 1]
					});
					byte[] array = new byte[(int)toparse[i + 1]];
					int length = (i + 2 + (int)toparse[i + 1] > toparse.Length) ? (toparse.Length - (i + 2)) : ((int)toparse[i + 1]);
					Array.Copy(toparse, i + 2, array, 0, length);
					Asn1DerObject[] array4 = asn1DerObject.objects.ToArray();
					asn1DerObject.objects[array4.Length - 1].Data = array;
					i = i + 1 + asn1DerObject.objects[array4.Length - 1].Lenght;
					break;
				}
				default:
					if (type == Asn1Der.Type.Sequence)
					{
						byte[] array;
						if (asn1DerObject.Lenght == 0)
						{
							asn1DerObject.Type = Asn1Der.Type.Sequence;
							asn1DerObject.Lenght = toparse.Length - (i + 2);
							array = new byte[asn1DerObject.Lenght];
						}
						else
						{
							asn1DerObject.objects.Add(new Asn1DerObject
							{
								Type = Asn1Der.Type.Sequence,
								Lenght = (int)toparse[i + 1]
							});
							array = new byte[(int)toparse[i + 1]];
						}
						int length = (array.Length > toparse.Length - (i + 2)) ? (toparse.Length - (i + 2)) : array.Length;
						Array.Copy(toparse, i + 2, array, 0, length);
						asn1DerObject.objects.Add(this.Parse(array));
						i = i + 1 + (int)toparse[i + 1];
					}
					break;
				}
			}
			return asn1DerObject;
		}

		// Token: 0x02000032 RID: 50
		public enum Type
		{
			// Token: 0x040000BE RID: 190
			Sequence = 48,
			// Token: 0x040000BF RID: 191
			Integer = 2,
			// Token: 0x040000C0 RID: 192
			BitString,
			// Token: 0x040000C1 RID: 193
			OctetString,
			// Token: 0x040000C2 RID: 194
			Null,
			// Token: 0x040000C3 RID: 195
			ObjectIdentifier
		}
	}
}
