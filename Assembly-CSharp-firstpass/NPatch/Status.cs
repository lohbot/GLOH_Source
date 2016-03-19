using System;

namespace NPatch
{
	public struct Status
	{
		public string totalStatus;

		public long totalDownloadSize;

		public long totalDownloadProcessedSize;

		public long totalInstallSize;

		public long totalInstallProcessedSize;

		public long totalSize;

		public long totalProcessedSize;

		public double totalProcessedPercent;

		public string taskStatus;

		public long taskSize;

		public long taskProcessedSize;

		public int taskReconnectCount;

		public int taskReconnectCountMax;

		public TASKTYPE taskType;

		public int totalTaskCount;

		public int totalTaskProcessedCount;

		public int totalPackCount;

		public int totalProcessedPackCount;

		public long packDownloadSize;

		public long packDownloadProcessedSize;

		public long packInstallSize;

		public long packInstallProcessedSize;

		public int fullPackCount;

		private void WriteStatus(string status, int step, int total)
		{
			if (total == 0)
			{
				Launcher.__Output(status);
			}
			else
			{
				Launcher.__Output(string.Concat(new object[]
				{
					status,
					"( ",
					step,
					" / ",
					total,
					" )"
				}));
			}
		}

		public void Clear()
		{
			this.totalStatus = string.Empty;
			this.totalDownloadSize = 0L;
			this.totalDownloadProcessedSize = 0L;
			this.totalInstallSize = 0L;
			this.totalInstallProcessedSize = 0L;
			this.totalSize = 0L;
			this.totalProcessedSize = 0L;
			this.taskStatus = string.Empty;
			this.taskSize = 0L;
			this.taskProcessedSize = 0L;
			this.taskReconnectCount = 0;
			this.taskReconnectCountMax = 0;
			this.taskType = TASKTYPE.NONE;
			this.totalTaskCount = 0;
			this.totalTaskProcessedCount = 0;
			this.totalPackCount = 0;
			this.totalProcessedPackCount = 0;
			this.packDownloadSize = 0L;
			this.packDownloadProcessedSize = 0L;
			this.packInstallSize = 0L;
			this.packInstallProcessedSize = 0L;
		}
	}
}
