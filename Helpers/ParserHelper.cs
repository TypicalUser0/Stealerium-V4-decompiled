using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace client.Helpers
{
	// Token: 0x0200001E RID: 30
	internal class ParserHelper
	{
		// Token: 0x060000BA RID: 186 RVA: 0x00002DDC File Offset: 0x00000FDC
		public static List<string> ParseFolders(string root, int depth, string contains)
		{
			List<string> list = new List<string>();
			try
			{
				foreach (string text in ParserHelper.EnumerateDirectories(root, "*", SearchOption.TopDirectoryOnly))
				{
					if (File.Exists(Path.Combine(text, contains)))
					{
						list.Add(text);
					}
					if (depth > 0)
					{
						list.AddRange(ParserHelper.ParseFolders(text, depth - 1, contains));
					}
				}
			}
			catch
			{
			}
			return list;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00002E6C File Offset: 0x0000106C
		public static List<string> FullDirList(DirectoryInfo dir, string searchPattern, int maxDepth)
		{
			List<string> list = new List<string>();
			if (maxDepth == 0)
			{
				return list;
			}
			try
			{
				foreach (FileInfo fileInfo in dir.GetFiles(searchPattern))
				{
					list.Add(fileInfo.FullName);
				}
			}
			catch (UnauthorizedAccessException)
			{
				return list;
			}
			foreach (DirectoryInfo dir2 in dir.GetDirectories())
			{
				list.AddRange(ParserHelper.FullDirList(dir2, searchPattern, maxDepth - 1));
			}
			return list;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00002EF8 File Offset: 0x000010F8
		private static IEnumerable<string> EnumerateDirectories(string parentDirectory, string searchPattern, SearchOption searchOpt)
		{
			IEnumerable<string> result;
			try
			{
				IEnumerable<string> first = Enumerable.Empty<string>();
				if (searchOpt == SearchOption.AllDirectories)
				{
					first = Directory.EnumerateDirectories(parentDirectory).SelectMany((string x) => ParserHelper.EnumerateDirectories(x, searchPattern, searchOpt));
				}
				result = first.Concat(Directory.EnumerateDirectories(parentDirectory, searchPattern));
			}
			catch (UnauthorizedAccessException)
			{
				result = Enumerable.Empty<string>();
			}
			return result;
		}
	}
}
