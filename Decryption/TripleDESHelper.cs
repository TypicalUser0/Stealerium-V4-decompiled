using System;
using System.IO;
using System.Security.Cryptography;

namespace client.Decryption
{
	// Token: 0x02000038 RID: 56
	internal class TripleDESHelper
	{
		// Token: 0x0600016C RID: 364 RVA: 0x00006D0C File Offset: 0x00004F0C
		public static string DESCBC_Decrypt_String(byte[] key, byte[] iv, byte[] input)
		{
			string result = null;
			using (TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider())
			{
				tripleDESCryptoServiceProvider.Key = key;
				tripleDESCryptoServiceProvider.IV = iv;
				tripleDESCryptoServiceProvider.Mode = CipherMode.CBC;
				tripleDESCryptoServiceProvider.Padding = PaddingMode.None;
				ICryptoTransform transform = tripleDESCryptoServiceProvider.CreateDecryptor(tripleDESCryptoServiceProvider.Key, tripleDESCryptoServiceProvider.IV);
				using (MemoryStream memoryStream = new MemoryStream(input))
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read))
					{
						using (StreamReader streamReader = new StreamReader(cryptoStream))
						{
							result = streamReader.ReadToEnd();
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00006DD8 File Offset: 0x00004FD8
		public static byte[] DESCBC_Decrypt_Byte(byte[] key, byte[] iv, byte[] input)
		{
			byte[] array = new byte[512];
			using (TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider())
			{
				tripleDESCryptoServiceProvider.Key = key;
				tripleDESCryptoServiceProvider.IV = iv;
				tripleDESCryptoServiceProvider.Mode = CipherMode.CBC;
				tripleDESCryptoServiceProvider.Padding = PaddingMode.None;
				ICryptoTransform transform = tripleDESCryptoServiceProvider.CreateDecryptor(tripleDESCryptoServiceProvider.Key, tripleDESCryptoServiceProvider.IV);
				using (MemoryStream memoryStream = new MemoryStream(input))
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read))
					{
						cryptoStream.Read(array, 0, array.Length);
					}
				}
			}
			return array;
		}
	}
}
