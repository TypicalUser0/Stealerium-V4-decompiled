using System;
using System.Runtime.InteropServices;

namespace client.Decryption
{
	// Token: 0x0200002A RID: 42
	internal class BCRYPT
	{
		// Token: 0x0600011A RID: 282
		[DllImport("BCRYPT.dll", EntryPoint = "BCryptOpenAlgorithmProvider")]
		public static extern uint method1(out IntPtr phAlgorithm, [MarshalAs(UnmanagedType.LPWStr)] string pszAlgId, [MarshalAs(UnmanagedType.LPWStr)] string pszImplementation, uint dwFlags);

		// Token: 0x0600011B RID: 283
		[DllImport("BCRYPT.dll", EntryPoint = "BCryptCloseAlgorithmProvider")]
		public static extern uint method2(IntPtr hAlgorithm, uint flags);

		// Token: 0x0600011C RID: 284
		[DllImport("BCRYPT.dll", EntryPoint = "BCryptGetProperty")]
		public static extern uint method3(IntPtr hObject, [MarshalAs(UnmanagedType.LPWStr)] string pszProperty, byte[] pbOutput, int cbOutput, ref int pcbResult, uint flags);

		// Token: 0x0600011D RID: 285
		[DllImport("BCRYPT.dll", EntryPoint = "BCryptSetProperty")]
		internal static extern uint method4(IntPtr hObject, [MarshalAs(UnmanagedType.LPWStr)] string pszProperty, byte[] pbInput, int cbInput, int dwFlags);

		// Token: 0x0600011E RID: 286
		[DllImport("BCRYPT.dll", EntryPoint = "BCryptImportKey")]
		public static extern uint method5(IntPtr hAlgorithm, IntPtr hImportKey, [MarshalAs(UnmanagedType.LPWStr)] string pszBlobType, out IntPtr phKey, IntPtr pbKeyObject, int cbKeyObject, byte[] pbInput, int cbInput, uint dwFlags);

		// Token: 0x0600011F RID: 287
		[DllImport("BCRYPT.dll", EntryPoint = "BCryptDestroyKey")]
		public static extern uint method6(IntPtr hKey);

		// Token: 0x06000120 RID: 288
		[DllImport("BCRYPT.dll", EntryPoint = "BCryptDecrypt")]
		internal static extern uint method7(IntPtr hKey, byte[] pbInput, int cbInput, ref BCRYPT.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO pPaddingInfo, byte[] pbIV, int cbIV, byte[] pbOutput, int cbOutput, ref int pcbResult, int dwFlags);

		// Token: 0x04000093 RID: 147
		public const uint ERROR_SUCCESS = 0U;

		// Token: 0x04000094 RID: 148
		public const uint BCRYPT_PAD_PSS = 8U;

		// Token: 0x04000095 RID: 149
		public const uint BCRYPT_PAD_OAEP = 4U;

		// Token: 0x04000096 RID: 150
		public static readonly byte[] BCRYPT_KEY_DATA_BLOB_MAGIC = BitConverter.GetBytes(1296188491);

		// Token: 0x04000097 RID: 151
		public static readonly string BCRYPT_OBJECT_LENGTH = "ObjectLength";

		// Token: 0x04000098 RID: 152
		public static readonly string BCRYPT_CHAIN_MODE_GCM = "ChainingModeGCM";

		// Token: 0x04000099 RID: 153
		public static readonly string BCRYPT_AUTH_TAG_LENGTH = "AuthTagLength";

		// Token: 0x0400009A RID: 154
		public static readonly string BCRYPT_CHAINING_MODE = "ChainingMode";

		// Token: 0x0400009B RID: 155
		public static readonly string BCRYPT_KEY_DATA_BLOB = "KeyDataBlob";

		// Token: 0x0400009C RID: 156
		public static readonly string BCRYPT_AES_ALGORITHM = "AES";

		// Token: 0x0400009D RID: 157
		public static readonly string MS_PRIMITIVE_PROVIDER = "Microsoft Primitive Provider";

		// Token: 0x0400009E RID: 158
		public static readonly int BCRYPT_AUTH_MODE_CHAIN_CALLS_FLAG = 1;

		// Token: 0x0400009F RID: 159
		public static readonly int BCRYPT_INIT_AUTH_MODE_INFO_VERSION = 1;

		// Token: 0x040000A0 RID: 160
		public static readonly uint STATUS_AUTH_TAG_MISMATCH = 3221266434U;

		// Token: 0x0200002B RID: 43
		public struct BCRYPT_PSS_PADDING_INFO
		{
			// Token: 0x06000123 RID: 291 RVA: 0x00005128 File Offset: 0x00003328
			public BCRYPT_PSS_PADDING_INFO(string pszAlgId, int cbSalt)
			{
				this.pszAlgId = pszAlgId;
				this.cbSalt = cbSalt;
			}

			// Token: 0x040000A1 RID: 161
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pszAlgId;

			// Token: 0x040000A2 RID: 162
			public int cbSalt;
		}

		// Token: 0x0200002C RID: 44
		public struct BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO : IDisposable
		{
			// Token: 0x06000124 RID: 292 RVA: 0x00005138 File Offset: 0x00003338
			public BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO(byte[] iv, byte[] aad, byte[] tag)
			{
				this = default(BCRYPT.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO);
				this.dwInfoVersion = BCRYPT.BCRYPT_INIT_AUTH_MODE_INFO_VERSION;
				this.cbSize = Marshal.SizeOf(typeof(BCRYPT.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO));
				if (iv != null)
				{
					this.cbNonce = iv.Length;
					this.pbNonce = Marshal.AllocHGlobal(this.cbNonce);
					Marshal.Copy(iv, 0, this.pbNonce, this.cbNonce);
				}
				if (aad != null)
				{
					this.cbAuthData = aad.Length;
					this.pbAuthData = Marshal.AllocHGlobal(this.cbAuthData);
					Marshal.Copy(aad, 0, this.pbAuthData, this.cbAuthData);
				}
				if (tag != null)
				{
					this.cbTag = tag.Length;
					this.pbTag = Marshal.AllocHGlobal(this.cbTag);
					Marshal.Copy(tag, 0, this.pbTag, this.cbTag);
					this.cbMacContext = tag.Length;
					this.pbMacContext = Marshal.AllocHGlobal(this.cbMacContext);
				}
			}

			// Token: 0x06000125 RID: 293 RVA: 0x00005218 File Offset: 0x00003418
			public void Dispose()
			{
				if (this.pbNonce != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(this.pbNonce);
				}
				if (this.pbTag != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(this.pbTag);
				}
				if (this.pbAuthData != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(this.pbAuthData);
				}
				if (this.pbMacContext != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(this.pbMacContext);
				}
			}

			// Token: 0x040000A3 RID: 163
			public int cbSize;

			// Token: 0x040000A4 RID: 164
			public int dwInfoVersion;

			// Token: 0x040000A5 RID: 165
			public IntPtr pbNonce;

			// Token: 0x040000A6 RID: 166
			public int cbNonce;

			// Token: 0x040000A7 RID: 167
			public IntPtr pbAuthData;

			// Token: 0x040000A8 RID: 168
			public int cbAuthData;

			// Token: 0x040000A9 RID: 169
			public IntPtr pbTag;

			// Token: 0x040000AA RID: 170
			public int cbTag;

			// Token: 0x040000AB RID: 171
			public IntPtr pbMacContext;

			// Token: 0x040000AC RID: 172
			public int cbMacContext;

			// Token: 0x040000AD RID: 173
			public int cbAAD;

			// Token: 0x040000AE RID: 174
			public long cbData;

			// Token: 0x040000AF RID: 175
			public int dwFlags;
		}

		// Token: 0x0200002D RID: 45
		public struct BCRYPT_KEY_LENGTHS_STRUCT
		{
			// Token: 0x040000B0 RID: 176
			public int dwMinLength;

			// Token: 0x040000B1 RID: 177
			public int dwMaxLength;

			// Token: 0x040000B2 RID: 178
			public int dwIncrement;
		}

		// Token: 0x0200002E RID: 46
		public struct BCRYPT_OAEP_PADDING_INFO
		{
			// Token: 0x06000126 RID: 294 RVA: 0x00005299 File Offset: 0x00003499
			public BCRYPT_OAEP_PADDING_INFO(string alg)
			{
				this.pszAlgId = alg;
				this.pbLabel = IntPtr.Zero;
				this.cbLabel = 0;
			}

			// Token: 0x040000B3 RID: 179
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pszAlgId;

			// Token: 0x040000B4 RID: 180
			public IntPtr pbLabel;

			// Token: 0x040000B5 RID: 181
			public int cbLabel;
		}
	}
}
