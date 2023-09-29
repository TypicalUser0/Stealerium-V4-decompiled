using System;
using System.Security.Cryptography;

namespace client.Decryption
{
	// Token: 0x02000035 RID: 53
	public class PBE
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00006331 File Offset: 0x00004531
		// (set) Token: 0x06000146 RID: 326 RVA: 0x00006339 File Offset: 0x00004539
		private byte[] ciphertext { get; set; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00006342 File Offset: 0x00004542
		// (set) Token: 0x06000148 RID: 328 RVA: 0x0000634A File Offset: 0x0000454A
		private byte[] globalsalt { get; set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000149 RID: 329 RVA: 0x00006353 File Offset: 0x00004553
		// (set) Token: 0x0600014A RID: 330 RVA: 0x0000635B File Offset: 0x0000455B
		private byte[] masterpass { get; set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600014B RID: 331 RVA: 0x00006364 File Offset: 0x00004564
		// (set) Token: 0x0600014C RID: 332 RVA: 0x0000636C File Offset: 0x0000456C
		private byte[] entrysalt { get; set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600014D RID: 333 RVA: 0x00006375 File Offset: 0x00004575
		// (set) Token: 0x0600014E RID: 334 RVA: 0x0000637D File Offset: 0x0000457D
		public byte[] partIV { get; private set; }

		// Token: 0x0600014F RID: 335 RVA: 0x00006386 File Offset: 0x00004586
		public PBE(byte[] _ciphertext, byte[] _globalsalt, byte[] _masterpassword, byte[] _entrysalt, byte[] _partiv)
		{
			this.ciphertext = _ciphertext;
			this.globalsalt = _globalsalt;
			this.masterpass = _masterpassword;
			this.entrysalt = _entrysalt;
			this.partIV = _partiv;
		}

		// Token: 0x06000150 RID: 336 RVA: 0x000063B4 File Offset: 0x000045B4
		public byte[] Compute()
		{
			int iterations = 1;
			int count = 32;
			byte[] array = new byte[this.globalsalt.Length + this.masterpass.Length];
			Buffer.BlockCopy(this.globalsalt, 0, array, 0, this.globalsalt.Length);
			Buffer.BlockCopy(this.masterpass, 0, array, this.globalsalt.Length, this.masterpass.Length);
			byte[] password = new SHA1Managed().ComputeHash(array);
			byte[] array2 = new byte[]
			{
				4,
				14
			};
			byte[] array3 = new byte[array2.Length + this.partIV.Length];
			Buffer.BlockCopy(array2, 0, array3, 0, array2.Length);
			Buffer.BlockCopy(this.partIV, 0, array3, array2.Length, this.partIV.Length);
			byte[] bytes = new PBKDF2(new HMACSHA256(), password, this.entrysalt, iterations).GetBytes(count);
			return new AesManaged
			{
				Mode = CipherMode.CBC,
				BlockSize = 128,
				KeySize = 256,
				Padding = PaddingMode.Zeros
			}.CreateDecryptor(bytes, array3).TransformFinalBlock(this.ciphertext, 0, this.ciphertext.Length);
		}
	}
}
