using System;
using System.Security.Cryptography;

namespace client.Decryption
{
	// Token: 0x02000036 RID: 54
	public class PBKDF2
	{
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000151 RID: 337 RVA: 0x000064C8 File Offset: 0x000046C8
		// (set) Token: 0x06000152 RID: 338 RVA: 0x000064D0 File Offset: 0x000046D0
		public HMAC Algorithm { get; private set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000153 RID: 339 RVA: 0x000064D9 File Offset: 0x000046D9
		// (set) Token: 0x06000154 RID: 340 RVA: 0x000064E1 File Offset: 0x000046E1
		public byte[] Salt { get; private set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000155 RID: 341 RVA: 0x000064EA File Offset: 0x000046EA
		// (set) Token: 0x06000156 RID: 342 RVA: 0x000064F2 File Offset: 0x000046F2
		public int IterationCount { get; private set; }

		// Token: 0x06000157 RID: 343 RVA: 0x000064FC File Offset: 0x000046FC
		public PBKDF2(HMAC algorithm, byte[] password, byte[] salt, int iterations)
		{
			if (algorithm == null)
			{
				throw new ArgumentNullException("algorithm", "Algorithm cannot be null.");
			}
			if (salt == null)
			{
				throw new ArgumentNullException("salt", "Salt cannot be null.");
			}
			if (password == null)
			{
				throw new ArgumentNullException("password", "Password cannot be null.");
			}
			this.Algorithm = algorithm;
			this.Algorithm.Key = password;
			this.Salt = salt;
			this.IterationCount = iterations;
			this.BlockSize = this.Algorithm.HashSize / 8;
			this.BufferBytes = new byte[this.BlockSize];
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00006595 File Offset: 0x00004795
		public PBKDF2(HMAC algorithm, byte[] password, byte[] salt) : this(algorithm, password, salt, 1000)
		{
		}

		// Token: 0x06000159 RID: 345 RVA: 0x000065A8 File Offset: 0x000047A8
		public byte[] GetBytes(int count)
		{
			byte[] array = new byte[count];
			int i = 0;
			int num = this.BufferEndIndex - this.BufferStartIndex;
			if (num > 0)
			{
				if (count < num)
				{
					Buffer.BlockCopy(this.BufferBytes, this.BufferStartIndex, array, 0, count);
					this.BufferStartIndex += count;
					return array;
				}
				Buffer.BlockCopy(this.BufferBytes, this.BufferStartIndex, array, 0, num);
				this.BufferStartIndex = (this.BufferEndIndex = 0);
				i += num;
			}
			while (i < count)
			{
				int num2 = count - i;
				this.BufferBytes = this.Func();
				if (num2 <= this.BlockSize)
				{
					Buffer.BlockCopy(this.BufferBytes, 0, array, i, num2);
					this.BufferStartIndex = num2;
					this.BufferEndIndex = this.BlockSize;
					return array;
				}
				Buffer.BlockCopy(this.BufferBytes, 0, array, i, this.BlockSize);
				i += this.BlockSize;
			}
			return array;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000668C File Offset: 0x0000488C
		private byte[] Func()
		{
			byte[] array = new byte[this.Salt.Length + 4];
			Buffer.BlockCopy(this.Salt, 0, array, 0, this.Salt.Length);
			Buffer.BlockCopy(PBKDF2.GetBytesFromInt(this.BlockIndex), 0, array, this.Salt.Length, 4);
			byte[] array2 = this.Algorithm.ComputeHash(array);
			byte[] array3 = array2;
			for (int i = 2; i <= this.IterationCount; i++)
			{
				array2 = this.Algorithm.ComputeHash(array2, 0, array2.Length);
				for (int j = 0; j < this.BlockSize; j++)
				{
					array3[j] ^= array2[j];
				}
			}
			if (this.BlockIndex == 4294967295U)
			{
				throw new InvalidOperationException("Derived key too long.");
			}
			this.BlockIndex += 1U;
			return array3;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00006754 File Offset: 0x00004954
		private static byte[] GetBytesFromInt(uint i)
		{
			byte[] bytes = BitConverter.GetBytes(i);
			if (BitConverter.IsLittleEndian)
			{
				return new byte[]
				{
					bytes[3],
					bytes[2],
					bytes[1],
					bytes[0]
				};
			}
			return bytes;
		}

		// Token: 0x040000CF RID: 207
		private readonly int BlockSize;

		// Token: 0x040000D0 RID: 208
		private uint BlockIndex = 1U;

		// Token: 0x040000D1 RID: 209
		private byte[] BufferBytes;

		// Token: 0x040000D2 RID: 210
		private int BufferStartIndex;

		// Token: 0x040000D3 RID: 211
		private int BufferEndIndex;
	}
}
