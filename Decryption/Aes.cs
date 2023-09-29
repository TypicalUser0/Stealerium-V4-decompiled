using System;
using System.Runtime.InteropServices;
using System.Text;
using client.Helpers;

namespace client.Decryption
{
	// Token: 0x02000029 RID: 41
	internal class Aes
	{
		// Token: 0x06000112 RID: 274 RVA: 0x00004D30 File Offset: 0x00002F30
		public byte[] Decrypt(byte[] key, byte[] iv, byte[] aad, byte[] cipherText, byte[] authTag)
		{
			IntPtr intPtr = this.OpenAlgorithmProvider(BCRYPT.BCRYPT_AES_ALGORITHM, BCRYPT.MS_PRIMITIVE_PROVIDER, BCRYPT.BCRYPT_CHAIN_MODE_GCM);
			IntPtr hKey;
			IntPtr hglobal = this.ImportKey(intPtr, key, out hKey);
			BCRYPT.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO bcrypt_AUTHENTICATED_CIPHER_MODE_INFO = new BCRYPT.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO(iv, aad, authTag);
			byte[] array2;
			using (bcrypt_AUTHENTICATED_CIPHER_MODE_INFO)
			{
				byte[] array = new byte[this.MaxAuthTagSize(intPtr)];
				int num = 0;
				uint num2 = BCRYPT.method7(hKey, cipherText, cipherText.Length, ref bcrypt_AUTHENTICATED_CIPHER_MODE_INFO, array, array.Length, null, 0, ref num, 0);
				if (num2 != 0U)
				{
					return null;
				}
				array2 = new byte[num];
				num2 = BCRYPT.method7(hKey, cipherText, cipherText.Length, ref bcrypt_AUTHENTICATED_CIPHER_MODE_INFO, array, array.Length, array2, array2.Length, ref num, 0);
				if (num2 == BCRYPT.STATUS_AUTH_TAG_MISMATCH)
				{
					return null;
				}
				if (num2 != 0U)
				{
					return null;
				}
			}
			BCRYPT.method6(hKey);
			Marshal.FreeHGlobal(hglobal);
			BCRYPT.method2(intPtr, 0U);
			return array2;
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00004E1C File Offset: 0x0000301C
		private int MaxAuthTagSize(IntPtr hAlg)
		{
			byte[] property = this.GetProperty(hAlg, BCRYPT.BCRYPT_AUTH_TAG_LENGTH);
			return BitConverter.ToInt32(new byte[]
			{
				property[4],
				property[5],
				property[6],
				property[7]
			}, 0);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00004E5C File Offset: 0x0000305C
		private IntPtr OpenAlgorithmProvider(string alg, string provider, string chainingMode)
		{
			IntPtr zero = IntPtr.Zero;
			if (BCRYPT.method1(out zero, alg, provider, 0U) != 0U)
			{
				return zero;
			}
			byte[] bytes = Encoding.Unicode.GetBytes(chainingMode);
			BCRYPT.method4(zero, BCRYPT.BCRYPT_CHAINING_MODE, bytes, bytes.Length, 0);
			return zero;
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00004E9C File Offset: 0x0000309C
		private IntPtr ImportKey(IntPtr hAlg, byte[] key, out IntPtr hKey)
		{
			int num = BitConverter.ToInt32(this.GetProperty(hAlg, BCRYPT.BCRYPT_OBJECT_LENGTH), 0);
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			byte[] array = this.Concat(new byte[][]
			{
				BCRYPT.BCRYPT_KEY_DATA_BLOB_MAGIC,
				BitConverter.GetBytes(1),
				BitConverter.GetBytes(key.Length),
				key
			});
			if (BCRYPT.method5(hAlg, IntPtr.Zero, BCRYPT.BCRYPT_KEY_DATA_BLOB, out hKey, intPtr, num, array, array.Length, 0U) != 0U)
			{
				return IntPtr.Zero;
			}
			return intPtr;
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00004F14 File Offset: 0x00003114
		private byte[] GetProperty(IntPtr hAlg, string name)
		{
			int num = 0;
			if (BCRYPT.method3(hAlg, name, null, 0, ref num, 0U) != 0U)
			{
				return null;
			}
			byte[] array = new byte[num];
			if (BCRYPT.method3(hAlg, name, array, array.Length, ref num, 0U) != 0U)
			{
				return null;
			}
			return array;
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00004F50 File Offset: 0x00003150
		public byte[] Concat(params byte[][] arrays)
		{
			int num = 0;
			foreach (byte[] array in arrays)
			{
				if (array != null)
				{
					num += array.Length;
				}
			}
			byte[] array2 = new byte[num - 1 + 1];
			int num2 = 0;
			foreach (byte[] array3 in arrays)
			{
				if (array3 != null)
				{
					Buffer.BlockCopy(array3, 0, array2, num2, array3.Length);
					num2 += array3.Length;
				}
			}
			return array2;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00004FC8 File Offset: 0x000031C8
		public static string DecryptValue(string base_str, byte[] key)
		{
			if (base_str.StartsWith("v1"))
			{
				byte[] bytes = Encoding.Default.GetBytes(base_str);
				byte[] array = new byte[12];
				Array.Copy(bytes, 3, array, 0, 12);
				try
				{
					byte[] array2 = new byte[bytes.Length - 15];
					Array.Copy(bytes, 15, array2, 0, bytes.Length - 15);
					byte[] array3 = new byte[16];
					byte[] array4 = new byte[array2.Length - array3.Length];
					Array.Copy(array2, array2.Length - 16, array3, 0, 16);
					Array.Copy(array2, 0, array4, 0, array2.Length - array3.Length);
					byte[] bytes2 = new Aes().Decrypt(key, array, null, array4, array3);
					return Encoding.UTF8.GetString(bytes2);
				}
				catch
				{
					return null;
				}
			}
			return Encoding.UTF8.GetString(FileHelper.Decrypt(Encoding.Default.GetBytes(base_str), null));
		}
	}
}
