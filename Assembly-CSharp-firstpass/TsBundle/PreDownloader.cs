using System;
using System.Text;

namespace TsBundle
{
	public static class PreDownloader
	{
		public delegate void CachingReport(string listFileName, float cachedRate, StringBuilder reportLogText);

		public delegate void DownlaodToComplete(string listFileName, float cachedRate);

		public static bool Pause
		{
			get
			{
				return Option.PauseBGLOAD;
			}
			set
			{
				Option.PauseBGLOAD = value;
			}
		}

		public static float Delay
		{
			get
			{
				return Option.DownloadDelay;
			}
			set
			{
				Option.DownloadDelay = value;
			}
		}

		public static bool IsCached(string listFileName, bool useLogWritten, PreDownloader.CachingReport callback)
		{
			StringBuilder targetLog = (!useLogWritten) ? null : new StringBuilder(1024);
			BackgroundInstall backgroundInstall = new BackgroundInstall();
			return backgroundInstall._IsCached(listFileName, true, targetLog, callback);
		}

		public static bool IsCached(string listFileName, PreDownloader.CachingReport callback)
		{
			return PreDownloader.IsCached(listFileName, false, callback);
		}

		public static bool ReportNoCached(string listFileName, PreDownloader.CachingReport callback)
		{
			BackgroundInstall backgroundInstall = new BackgroundInstall();
			return backgroundInstall._IsCached(listFileName, false, new StringBuilder(1024), callback);
		}

		public static bool Request(string listFileName, PreDownloader.DownlaodToComplete callback)
		{
			if (!Option.EnablePreDownload)
			{
				TsLog.LogWarning("[PreDownload] TsBundle.Predownloader.Rqeuset({0}) => Disabled pre-download", new object[]
				{
					listFileName
				});
				return false;
			}
			BackgroundInstall backgroundInstall = new BackgroundInstall();
			return backgroundInstall._RequestPreDownload(listFileName, callback);
		}

		public static bool Request(string listFileName)
		{
			return PreDownloader.Request(listFileName, null);
		}

		public static float GetProgress(string listFileName)
		{
			BackgroundInstall.Progress progress;
			if (BackgroundInstall.ms_ProgressCollection.TryGetValue(listFileName, out progress))
			{
				return (float)progress.count / (float)progress.total;
			}
			return float.NegativeInfinity;
		}
	}
}
