using System;
using System.Collections.Generic;

namespace client.Helpers
{
	// Token: 0x0200001D RID: 29
	internal class LoggerHelper
	{
		// Token: 0x060000B7 RID: 183 RVA: 0x00002DC0 File Offset: 0x00000FC0
		public static void HandleRuntimeError(Exception e)
		{
			LoggerHelper.RuntimeErrors.Add(e);
		}

		// Token: 0x04000066 RID: 102
		public static List<Exception> RuntimeErrors = new List<Exception>();
	}
}
