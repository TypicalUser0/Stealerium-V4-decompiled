using System;
using System.IO;
using System.Text;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace client.Decryption
{
	// Token: 0x02000039 RID: 57
	internal class SQLHelper
	{
		// Token: 0x0600016F RID: 367 RVA: 0x00006E90 File Offset: 0x00005090
		public SQLHelper(byte[] data)
		{
			this.db_bytes = data;
			if (Encoding.Default.GetString(this.db_bytes, 0, 15).CompareTo("SQLite format 3") != 0)
			{
				throw new Exception("Not a valid SQLite 3 Database File");
			}
			if (this.db_bytes[52] != 0)
			{
				throw new Exception("Auto-vacuum capable database is not supported");
			}
			this.page_size = (ushort)this.ConvertToInteger(16, 2);
			this.encoding = this.ConvertToInteger(56, 4);
			if (decimal.Compare(new decimal(this.encoding), 0m) == 0)
			{
				this.encoding = 1UL;
			}
			this.ReadMasterTable(100UL);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00006F4C File Offset: 0x0000514C
		public SQLHelper(string baseName)
		{
			if (File.Exists(baseName))
			{
				FileSystem.FileOpen(1, baseName, OpenMode.Binary, OpenAccess.Read, OpenShare.Shared, -1);
				string s = Strings.Space((int)FileSystem.LOF(1));
				FileSystem.FileGet(1, ref s, -1L, false);
				FileSystem.FileClose(new int[]
				{
					1
				});
				this.db_bytes = Encoding.Default.GetBytes(s);
				if (Encoding.Default.GetString(this.db_bytes, 0, 15).CompareTo("SQLite format 3") != 0)
				{
					throw new Exception("Not a valid SQLite 3 Database File");
				}
				if (this.db_bytes[52] != 0)
				{
					throw new Exception("Auto-vacuum capable database is not supported");
				}
				this.page_size = (ushort)this.ConvertToInteger(16, 2);
				this.encoding = this.ConvertToInteger(56, 4);
				if (decimal.Compare(new decimal(this.encoding), 0m) == 0)
				{
					this.encoding = 1UL;
				}
				this.ReadMasterTable(100UL);
			}
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00007050 File Offset: 0x00005250
		private ulong ConvertToInteger(int startIndex, int Size)
		{
			if (Size > 8 | Size == 0)
			{
				return 0UL;
			}
			ulong num = 0UL;
			int num2 = Size - 1;
			for (int i = 0; i <= num2; i++)
			{
				num = (num << 8 | (ulong)this.db_bytes[startIndex + i]);
			}
			return num;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00007090 File Offset: 0x00005290
		private long CVL(int startIndex, int endIndex)
		{
			endIndex++;
			byte[] array = new byte[8];
			int num = endIndex - startIndex;
			bool flag = false;
			if (num == 0 | num > 9)
			{
				return 0L;
			}
			if (num == 1)
			{
				array[0] = (this.db_bytes[startIndex] & 127);
				return BitConverter.ToInt64(array, 0);
			}
			if (num == 9)
			{
				flag = true;
			}
			int num2 = 1;
			int num3 = 7;
			int num4 = 0;
			if (flag)
			{
				array[0] = this.db_bytes[endIndex - 1];
				endIndex--;
				num4 = 1;
			}
			for (int i = endIndex - 1; i >= startIndex; i += -1)
			{
				if (i - 1 >= startIndex)
				{
					array[num4] = (byte)(((int)((byte)(this.db_bytes[i] >> (num2 - 1 & 7))) & 255 >> num2) | (int)((byte)(this.db_bytes[i - 1] << (num3 & 7))));
					num2++;
					num4++;
					num3--;
				}
				else if (!flag)
				{
					array[num4] = (byte)((int)((byte)(this.db_bytes[i] >> (num2 - 1 & 7))) & 255 >> num2);
				}
			}
			return BitConverter.ToInt64(array, 0);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00007197 File Offset: 0x00005397
		public int GetRowCount()
		{
			return this.table_entries.Length;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x000071A4 File Offset: 0x000053A4
		public string[] GetTableNames()
		{
			string[] array = null;
			int num = 0;
			int num2 = this.master_table_entries.Length - 1;
			for (int i = 0; i <= num2; i++)
			{
				if (this.master_table_entries[i].item_type == "table")
				{
					array = (string[])Utils.CopyArray(array, new string[num + 1]);
					array[num] = this.master_table_entries[i].item_name;
					num++;
				}
			}
			return array;
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00007216 File Offset: 0x00005416
		public string GetValue(int row_num, int field)
		{
			if (row_num >= this.table_entries.Length)
			{
				return null;
			}
			if (field >= this.table_entries[row_num].content.Length)
			{
				return null;
			}
			return this.table_entries[row_num].content[field];
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00007250 File Offset: 0x00005450
		public string GetValue(int row_num, string field)
		{
			int num = -1;
			int num2 = this.field_names.Length - 1;
			for (int i = 0; i <= num2; i++)
			{
				if (this.field_names[i].ToLower().CompareTo(field.ToLower()) == 0)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				return null;
			}
			return this.GetValue(row_num, num);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x000072A4 File Offset: 0x000054A4
		private int GVL(int startIndex)
		{
			if (startIndex > this.db_bytes.Length)
			{
				return 0;
			}
			int num = startIndex + 8;
			for (int i = startIndex; i <= num; i++)
			{
				if (i > this.db_bytes.Length - 1)
				{
					return 0;
				}
				if ((this.db_bytes[i] & 128) != 128)
				{
					return i;
				}
			}
			return startIndex + 8;
		}

		// Token: 0x06000178 RID: 376 RVA: 0x000072F7 File Offset: 0x000054F7
		private bool IsOdd(long value)
		{
			return (value & 1L) == 1L;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00007304 File Offset: 0x00005504
		private void ReadMasterTable(ulong Offset)
		{
			if (this.db_bytes[(int)Offset] == 13)
			{
				ushort num = Convert.ToUInt16(decimal.Subtract(new decimal(this.ConvertToInteger(Convert.ToInt32(decimal.Add(new decimal(Offset), 3m)), 2)), 1m));
				int num2 = 0;
				if (this.master_table_entries != null)
				{
					num2 = this.master_table_entries.Length;
					this.master_table_entries = (SQLHelper.sqlite_master_entry[])Utils.CopyArray(this.master_table_entries, new SQLHelper.sqlite_master_entry[this.master_table_entries.Length + (int)num + 1]);
				}
				else
				{
					this.master_table_entries = new SQLHelper.sqlite_master_entry[(int)(num + 1)];
				}
				int num3 = (int)num;
				for (int i = 0; i <= num3; i++)
				{
					ulong num4 = this.ConvertToInteger(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(Offset), 8m), new decimal(i * 2))), 2);
					if (decimal.Compare(new decimal(Offset), 100m) != 0)
					{
						num4 += Offset;
					}
					int num5 = this.GVL((int)num4);
					this.CVL((int)num4, num5);
					int num6 = this.GVL(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), decimal.Subtract(new decimal(num5), new decimal(num4))), 1m)));
					this.master_table_entries[num2 + i].row_id = this.CVL(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), decimal.Subtract(new decimal(num5), new decimal(num4))), 1m)), num6);
					num4 = Convert.ToUInt64(decimal.Add(decimal.Add(new decimal(num4), decimal.Subtract(new decimal(num6), new decimal(num4))), 1m));
					num5 = this.GVL((int)num4);
					num6 = num5;
					long value = this.CVL((int)num4, num5);
					long[] array = new long[5];
					int num7 = 0;
					do
					{
						num5 = num6 + 1;
						num6 = this.GVL(num5);
						array[num7] = this.CVL(num5, num6);
						if (array[num7] > 9L)
						{
							if (this.IsOdd(array[num7]))
							{
								array[num7] = (long)Math.Round((double)(array[num7] - 13L) / 2.0);
							}
							else
							{
								array[num7] = (long)Math.Round((double)(array[num7] - 12L) / 2.0);
							}
						}
						else
						{
							array[num7] = (long)((ulong)this.SQLDataTypeSize[(int)array[num7]]);
						}
						num7++;
					}
					while (num7 <= 4);
					if (decimal.Compare(new decimal(this.encoding), 1m) == 0)
					{
						this.master_table_entries[num2 + i].item_type = Encoding.Default.GetString(this.db_bytes, Convert.ToInt32(decimal.Add(new decimal(num4), new decimal(value))), (int)array[0]);
					}
					else if (decimal.Compare(new decimal(this.encoding), 2m) == 0)
					{
						this.master_table_entries[num2 + i].item_type = Encoding.Unicode.GetString(this.db_bytes, Convert.ToInt32(decimal.Add(new decimal(num4), new decimal(value))), (int)array[0]);
					}
					else if (decimal.Compare(new decimal(this.encoding), 3m) == 0)
					{
						this.master_table_entries[num2 + i].item_type = Encoding.BigEndianUnicode.GetString(this.db_bytes, Convert.ToInt32(decimal.Add(new decimal(num4), new decimal(value))), (int)array[0]);
					}
					if (decimal.Compare(new decimal(this.encoding), 1m) == 0)
					{
						this.master_table_entries[num2 + i].item_name = Encoding.Default.GetString(this.db_bytes, Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), new decimal(value)), new decimal(array[0]))), (int)array[1]);
					}
					else if (decimal.Compare(new decimal(this.encoding), 2m) == 0)
					{
						this.master_table_entries[num2 + i].item_name = Encoding.Unicode.GetString(this.db_bytes, Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), new decimal(value)), new decimal(array[0]))), (int)array[1]);
					}
					else if (decimal.Compare(new decimal(this.encoding), 3m) == 0)
					{
						this.master_table_entries[num2 + i].item_name = Encoding.BigEndianUnicode.GetString(this.db_bytes, Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), new decimal(value)), new decimal(array[0]))), (int)array[1]);
					}
					this.master_table_entries[num2 + i].root_num = (long)this.ConvertToInteger(Convert.ToInt32(decimal.Add(decimal.Add(decimal.Add(decimal.Add(new decimal(num4), new decimal(value)), new decimal(array[0])), new decimal(array[1])), new decimal(array[2]))), (int)array[3]);
					if (decimal.Compare(new decimal(this.encoding), 1m) == 0)
					{
						this.master_table_entries[num2 + i].sql_statement = Encoding.Default.GetString(this.db_bytes, Convert.ToInt32(decimal.Add(decimal.Add(decimal.Add(decimal.Add(decimal.Add(new decimal(num4), new decimal(value)), new decimal(array[0])), new decimal(array[1])), new decimal(array[2])), new decimal(array[3]))), (int)array[4]);
					}
					else if (decimal.Compare(new decimal(this.encoding), 2m) == 0)
					{
						this.master_table_entries[num2 + i].sql_statement = Encoding.Unicode.GetString(this.db_bytes, Convert.ToInt32(decimal.Add(decimal.Add(decimal.Add(decimal.Add(decimal.Add(new decimal(num4), new decimal(value)), new decimal(array[0])), new decimal(array[1])), new decimal(array[2])), new decimal(array[3]))), (int)array[4]);
					}
					else if (decimal.Compare(new decimal(this.encoding), 3m) == 0)
					{
						this.master_table_entries[num2 + i].sql_statement = Encoding.BigEndianUnicode.GetString(this.db_bytes, Convert.ToInt32(decimal.Add(decimal.Add(decimal.Add(decimal.Add(decimal.Add(new decimal(num4), new decimal(value)), new decimal(array[0])), new decimal(array[1])), new decimal(array[2])), new decimal(array[3]))), (int)array[4]);
					}
				}
				return;
			}
			if (this.db_bytes[(int)Offset] == 5)
			{
				int num8 = (int)Convert.ToUInt16(decimal.Subtract(new decimal(this.ConvertToInteger(Convert.ToInt32(decimal.Add(new decimal(Offset), 3m)), 2)), 1m));
				for (int j = 0; j <= num8; j++)
				{
					ushort num9 = (ushort)this.ConvertToInteger(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(Offset), 12m), new decimal(j * 2))), 2);
					if (decimal.Compare(new decimal(Offset), 100m) == 0)
					{
						this.ReadMasterTable(Convert.ToUInt64(decimal.Multiply(decimal.Subtract(new decimal(this.ConvertToInteger((int)num9, 4)), 1m), new decimal((int)this.page_size))));
					}
					else
					{
						this.ReadMasterTable(Convert.ToUInt64(decimal.Multiply(decimal.Subtract(new decimal(this.ConvertToInteger((int)(Offset + (ulong)num9), 4)), 1m), new decimal((int)this.page_size))));
					}
				}
				this.ReadMasterTable(Convert.ToUInt64(decimal.Multiply(decimal.Subtract(new decimal(this.ConvertToInteger(Convert.ToInt32(decimal.Add(new decimal(Offset), 8m)), 4)), 1m), new decimal((int)this.page_size))));
			}
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00007B30 File Offset: 0x00005D30
		public bool ReadTable(string TableName)
		{
			int num = -1;
			int num2 = this.master_table_entries.Length - 1;
			for (int i = 0; i <= num2; i++)
			{
				if (this.master_table_entries[i].item_name.ToLower().CompareTo(TableName.ToLower()) == 0)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				return false;
			}
			string[] array = this.master_table_entries[num].sql_statement.Substring(this.master_table_entries[num].sql_statement.IndexOf("(") + 1).Split(new char[]
			{
				','
			});
			int num3 = array.Length - 1;
			for (int j = 0; j <= num3; j++)
			{
				array[j] = array[j].TrimStart(Array.Empty<char>());
				int num4 = array[j].IndexOf(" ");
				if (num4 > 0)
				{
					array[j] = array[j].Substring(0, num4);
				}
				if (array[j].IndexOf("UNIQUE") == 0)
				{
					break;
				}
				this.field_names = (string[])Utils.CopyArray(this.field_names, new string[j + 1]);
				this.field_names[j] = array[j];
			}
			return this.ReadTableFromOffset((ulong)((this.master_table_entries[num].root_num - 1L) * (long)((ulong)this.page_size)));
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00007C78 File Offset: 0x00005E78
		private bool ReadTableFromOffset(ulong Offset)
		{
			if (this.db_bytes[(int)Offset] == 13)
			{
				int num = Convert.ToInt32(decimal.Subtract(new decimal(this.ConvertToInteger(Convert.ToInt32(decimal.Add(new decimal(Offset), 3m)), 2)), 1m));
				int num2 = 0;
				if (this.table_entries != null)
				{
					num2 = this.table_entries.Length;
					this.table_entries = (SQLHelper.table_entry[])Utils.CopyArray(this.table_entries, new SQLHelper.table_entry[this.table_entries.Length + num + 1]);
				}
				else
				{
					this.table_entries = new SQLHelper.table_entry[num + 1];
				}
				int num3 = num;
				for (int i = 0; i <= num3; i++)
				{
					SQLHelper.record_header_field[] array = null;
					ulong num4 = this.ConvertToInteger(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(Offset), 8m), new decimal(i * 2))), 2);
					if (decimal.Compare(new decimal(Offset), 100m) != 0)
					{
						num4 += Offset;
					}
					int num5 = this.GVL((int)num4);
					this.CVL((int)num4, num5);
					int num6 = this.GVL(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), decimal.Subtract(new decimal(num5), new decimal(num4))), 1m)));
					this.table_entries[num2 + i].row_id = this.CVL(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), decimal.Subtract(new decimal(num5), new decimal(num4))), 1m)), num6);
					num4 = Convert.ToUInt64(decimal.Add(decimal.Add(new decimal(num4), decimal.Subtract(new decimal(num6), new decimal(num4))), 1m));
					num5 = this.GVL((int)num4);
					num6 = num5;
					long num7 = this.CVL((int)num4, num5);
					long num8 = Convert.ToInt64(decimal.Add(decimal.Subtract(new decimal(num4), new decimal(num5)), 1m));
					int num9 = 0;
					while (num8 < num7)
					{
						array = (SQLHelper.record_header_field[])Utils.CopyArray(array, new SQLHelper.record_header_field[num9 + 1]);
						num5 = num6 + 1;
						num6 = this.GVL(num5);
						array[num9].type = this.CVL(num5, num6);
						if (array[num9].type > 9L)
						{
							if (this.IsOdd(array[num9].type))
							{
								array[num9].size = (long)Math.Round((double)(array[num9].type - 13L) / 2.0);
							}
							else
							{
								array[num9].size = (long)Math.Round((double)(array[num9].type - 12L) / 2.0);
							}
						}
						else
						{
							array[num9].size = (long)((ulong)this.SQLDataTypeSize[(int)array[num9].type]);
						}
						num8 = num8 + (long)(num6 - num5) + 1L;
						num9++;
					}
					this.table_entries[num2 + i].content = new string[array.Length - 1 + 1];
					int num10 = 0;
					int num11 = array.Length - 1;
					for (int j = 0; j <= num11; j++)
					{
						if (array[j].type > 9L)
						{
							if (!this.IsOdd(array[j].type))
							{
								if (decimal.Compare(new decimal(this.encoding), 1m) == 0)
								{
									this.table_entries[num2 + i].content[j] = Encoding.Default.GetString(this.db_bytes, Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), new decimal(num7)), new decimal(num10))), (int)array[j].size);
								}
								else if (decimal.Compare(new decimal(this.encoding), 2m) == 0)
								{
									this.table_entries[num2 + i].content[j] = Encoding.Unicode.GetString(this.db_bytes, Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), new decimal(num7)), new decimal(num10))), (int)array[j].size);
								}
								else if (decimal.Compare(new decimal(this.encoding), 3m) == 0)
								{
									this.table_entries[num2 + i].content[j] = Encoding.BigEndianUnicode.GetString(this.db_bytes, Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), new decimal(num7)), new decimal(num10))), (int)array[j].size);
								}
							}
							else
							{
								this.table_entries[num2 + i].content[j] = Encoding.Default.GetString(this.db_bytes, Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), new decimal(num7)), new decimal(num10))), (int)array[j].size);
							}
						}
						else
						{
							this.table_entries[num2 + i].content[j] = Conversions.ToString(this.ConvertToInteger(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), new decimal(num7)), new decimal(num10))), (int)array[j].size));
						}
						num10 += (int)array[j].size;
					}
				}
			}
			else if (this.db_bytes[(int)Offset] == 5)
			{
				int num12 = (int)Convert.ToUInt16(decimal.Subtract(new decimal(this.ConvertToInteger(Convert.ToInt32(decimal.Add(new decimal(Offset), 3m)), 2)), 1m));
				for (int k = 0; k <= num12; k++)
				{
					ushort num13 = (ushort)this.ConvertToInteger(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(Offset), 12m), new decimal(k * 2))), 2);
					this.ReadTableFromOffset(Convert.ToUInt64(decimal.Multiply(decimal.Subtract(new decimal(this.ConvertToInteger((int)(Offset + (ulong)num13), 4)), 1m), new decimal((int)this.page_size))));
				}
				this.ReadTableFromOffset(Convert.ToUInt64(decimal.Multiply(decimal.Subtract(new decimal(this.ConvertToInteger(Convert.ToInt32(decimal.Add(new decimal(Offset), 8m)), 4)), 1m), new decimal((int)this.page_size))));
			}
			return true;
		}

		// Token: 0x040000DD RID: 221
		private byte[] db_bytes;

		// Token: 0x040000DE RID: 222
		private ulong encoding;

		// Token: 0x040000DF RID: 223
		private string[] field_names;

		// Token: 0x040000E0 RID: 224
		private SQLHelper.sqlite_master_entry[] master_table_entries;

		// Token: 0x040000E1 RID: 225
		private ushort page_size;

		// Token: 0x040000E2 RID: 226
		private byte[] SQLDataTypeSize = new byte[]
		{
			0,
			1,
			2,
			3,
			4,
			6,
			8,
			8,
			0,
			0
		};

		// Token: 0x040000E3 RID: 227
		private SQLHelper.table_entry[] table_entries;

		// Token: 0x0200003A RID: 58
		private struct record_header_field
		{
			// Token: 0x040000E4 RID: 228
			public long size;

			// Token: 0x040000E5 RID: 229
			public long type;
		}

		// Token: 0x0200003B RID: 59
		private struct sqlite_master_entry
		{
			// Token: 0x040000E6 RID: 230
			public long row_id;

			// Token: 0x040000E7 RID: 231
			public string item_type;

			// Token: 0x040000E8 RID: 232
			public string item_name;

			// Token: 0x040000E9 RID: 233
			public string astable_name;

			// Token: 0x040000EA RID: 234
			public long root_num;

			// Token: 0x040000EB RID: 235
			public string sql_statement;
		}

		// Token: 0x0200003C RID: 60
		private struct table_entry
		{
			// Token: 0x040000EC RID: 236
			public long row_id;

			// Token: 0x040000ED RID: 237
			public string[] content;
		}
	}
}
