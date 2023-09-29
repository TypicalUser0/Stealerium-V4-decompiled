using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace client.Helpers
{
	// Token: 0x02000023 RID: 35
	internal class ZipHelper
	{
		// Token: 0x060000E2 RID: 226 RVA: 0x0000345A File Offset: 0x0000165A
		public static void AddStream(byte[] content, string path)
		{
			ZipHelper.files.Add(new CompressedFile
			{
				Name = path,
				Content = content
			});
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x0000347C File Offset: 0x0000167C
		public static byte[] CreateArchive()
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (ZipArchive zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, false))
				{
					foreach (CompressedFile compressedFile in ZipHelper.files)
					{
						ZipArchiveEntry zipArchiveEntry = zipArchive.CreateEntry(compressedFile.Name);
						using (MemoryStream memoryStream2 = new MemoryStream(compressedFile.Content))
						{
							using (Stream stream = zipArchiveEntry.Open())
							{
								memoryStream2.CopyTo(stream);
							}
						}
					}
				}
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x04000080 RID: 128
		private static List<CompressedFile> files = new List<CompressedFile>();
	}
}
