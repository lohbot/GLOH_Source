using System;

namespace TsBundle
{
	public static class UsingAssetRecorder
	{
		private static BackgroundInstall m_Instance;

		public static void Start(string listFileName)
		{
			if (UsingAssetRecorder.m_Instance != null)
			{
				UsingAssetRecorder.m_Instance.StopRecord();
			}
			UsingAssetRecorder.m_Instance = new BackgroundInstall();
			UsingAssetRecorder.m_Instance.StartRecord(listFileName);
		}

		public static void Stop()
		{
			if (UsingAssetRecorder.m_Instance != null)
			{
				UsingAssetRecorder.m_Instance.StopRecord();
				UsingAssetRecorder.m_Instance = null;
			}
		}

		internal static void RecordFile(string assetBundlePath)
		{
			if (UsingAssetRecorder.m_Instance != null)
			{
				UsingAssetRecorder.m_Instance.RecordFile(assetBundlePath);
			}
		}
	}
}
