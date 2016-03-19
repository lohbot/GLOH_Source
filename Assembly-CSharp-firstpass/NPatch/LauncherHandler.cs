using System;
using System.Collections.Generic;

namespace NPatch
{
	public class LauncherHandler
	{
		public virtual void OnProcessAtWorkThread(Status status)
		{
		}

		public virtual void OnProcessAtMainThread(Status status)
		{
			if (status.totalSize == 0L)
			{
				Console.WriteLine(status.totalStatus);
			}
			else if (status.taskType == TASKTYPE.DOWNLOADPACK)
			{
				Console.WriteLine(string.Concat(new object[]
				{
					"pack Download Size : ",
					status.packDownloadProcessedSize,
					" / ",
					status.packDownloadSize
				}));
			}
			else if (status.taskType == TASKTYPE.INSTALL)
			{
				Console.WriteLine(string.Concat(new object[]
				{
					"pack Install Size : ",
					status.packInstallProcessedSize,
					" / ",
					status.packInstallSize
				}));
				Console.WriteLine(string.Concat(new object[]
				{
					"Total Processed Pack Count : ",
					status.totalProcessedPackCount,
					" / ",
					status.totalPackCount
				}));
			}
		}

		public virtual void SetEdgeURL(ref string url_root)
		{
		}

		public virtual Launcher.Task.TaskResult OnCheckStart()
		{
			return Launcher.Task.TaskResult.SUCCESS;
		}

		public virtual void OnStartDownloadPack(string fileName, int order)
		{
		}

		public virtual void OnEndDownloadPack(string fileName, int order)
		{
		}

		public virtual void OnStartInstallPack(string fileName, int order)
		{
		}

		public virtual void OnEndInstallPack(string fileName, int order)
		{
		}

		public virtual void OnErrorFileVerifier(List<string> errorList)
		{
		}

		public virtual void OnFinish(ERRORLEVEL errorLevel, string errorString)
		{
		}
	}
}
