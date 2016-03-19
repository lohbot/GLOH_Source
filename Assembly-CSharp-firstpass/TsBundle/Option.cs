using System;
using UnityEngine;

namespace TsBundle
{
	public static class Option
	{
		public const int maxDonwnloadTokCnt = 3;

		private static bool ms_usePatchDir = true;

		private static bool ms_loadFromCacheOrDownload = false;

		private static string[] ms_protocol = new string[]
		{
			"file://",
			"http://",
			"ftp://"
		};

		private static string[] ms_rootPath = new string[3];

		private static bool ms_EnableTrace = Option.InitBundleTrace();

		private static bool ms_EnableReportCallStack = Option.InitBundleCallStack();

		private static string KEY_PREDOWNLOAD = "PreDownload";

		private static bool ms_EnablePreDownload = Option.InitPreDownload();

		private static string KEY_PREDOWNLOAD_HISTORY = "PreDownloadHistory";

		private static bool ms_EnablePreDownloadHistory = Option.InitPreDownloadHistory();

		private static bool m_pauseBGLOAD = false;

		private static float m_downlaodDelay = Option.InitDownloadDelay();

		private static bool m_verifyLoadedBundle = Option.InitVerifyLoadedBundle();

		private static bool ms_pause = false;

		public static string localRootAssetFolder
		{
			get
			{
				return "AssetBundles";
			}
		}

		public static string extScene
		{
			get
			{
				return ".unity3d";
			}
		}

		public static string extAsset
		{
			get
			{
				return ".assetbundle";
			}
		}

		public static string defaultStackName
		{
			get
			{
				return "DefaultCommon";
			}
		}

		public static string undefinedStackName
		{
			get
			{
				return "UndefinedGroup";
			}
		}

		public static string IndependentFromStageStackName
		{
			get
			{
				return "IndependentFromStage";
			}
		}

		public static string PredownloadMarkingFileName
		{
			get
			{
				return "PredownloadMarking";
			}
		}

		public static string currentStackName
		{
			get
			{
				return "Current";
			}
		}

		public static bool useCache
		{
			get
			{
				return Option.ms_loadFromCacheOrDownload;
			}
		}

		public static bool usePatchDir
		{
			get
			{
				return Option.ms_usePatchDir;
			}
			set
			{
				Option.ms_usePatchDir = value;
				TsLog.Log(" 111111111111111111111111 = " + value, new object[0]);
			}
		}

		public static bool localWWW
		{
			get;
			set;
		}

		public static bool EnableTrace
		{
			get
			{
				return Option.ms_EnableTrace;
			}
			set
			{
				Option.ms_EnableTrace = value;
				PlayerPrefs.SetInt("BundleTrace", (!Option.ms_EnableTrace) ? 0 : 1);
			}
		}

		public static bool EnableReportCallStatck
		{
			get
			{
				return Option.ms_EnableReportCallStack;
			}
			set
			{
				Option.ms_EnableReportCallStack = value;
				PlayerPrefs.SetInt("BundleCallStack", (!Option.ms_EnableReportCallStack) ? 0 : 1);
			}
		}

		public static bool EnablePreDownload
		{
			get
			{
				return Option.ms_EnablePreDownload;
			}
			set
			{
				Option.ms_EnablePreDownload = value;
				PlayerPrefs.SetInt(Option.KEY_PREDOWNLOAD, (!Option.ms_EnablePreDownload) ? 0 : 1);
			}
		}

		public static bool EnablePreDownloadHistory
		{
			get
			{
				return Option.ms_EnablePreDownloadHistory;
			}
			set
			{
				Option.ms_EnablePreDownloadHistory = value;
				PlayerPrefs.SetInt(Option.KEY_PREDOWNLOAD_HISTORY, (!Option.ms_EnablePreDownloadHistory) ? 0 : 1);
			}
		}

		public static bool PauseBGLOAD
		{
			get
			{
				return Option.m_pauseBGLOAD;
			}
			set
			{
				Option.m_pauseBGLOAD = value;
				TsLog.Log("[TsBundle] (PreDownlad => {0})", new object[]
				{
					(!Option.m_pauseBGLOAD) ? "Resume" : "Pause"
				});
			}
		}

		internal static float DownloadDelay
		{
			get
			{
				return Option.m_downlaodDelay;
			}
			set
			{
				Option.m_downlaodDelay = value;
				PlayerPrefs.SetFloat("DownloadDelay", Option.m_downlaodDelay);
			}
		}

		public static bool VerifyLoadedBundle
		{
			get
			{
				return Option.m_verifyLoadedBundle;
			}
			set
			{
				Option.m_verifyLoadedBundle = value;
				PlayerPrefs.SetInt("VerifyLoadedBundle", (!Option.m_verifyLoadedBundle) ? 0 : 1);
			}
		}

		public static bool isPause
		{
			get
			{
				return Option.ms_pause;
			}
		}

		public static void SetLoadFromCacheOrDownload(bool useLoadFromCacheOrDownload)
		{
			Option.ms_loadFromCacheOrDownload = useLoadFromCacheOrDownload;
		}

		public static bool IsLoadFromCacheOrDownload()
		{
			return Option.ms_loadFromCacheOrDownload;
		}

		public static void SetProtocolRootPath(Protocol prtcl, string rootPath)
		{
			Option.ms_rootPath[(int)prtcl] = rootPath;
			TsLog.LogWarning("SetProtocolRootPath[ {0} ] : {1}", new object[]
			{
				prtcl.ToString(),
				Option.GetProtocolRootPath(prtcl)
			});
		}

		public static string GetProtocolRootPath(Protocol prtcl)
		{
			string text = Option.ms_protocol[(int)prtcl];
			string text2 = Option.ms_rootPath[(int)prtcl];
			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2))
			{
				TsLog.LogError("TsBundle.Option is not initialized. You must call TsBundle.Option.SetProtocolRootPath() first. Protocol={0} Root={1}", new object[]
				{
					text.ToString(),
					text2
				});
				return "undefined";
			}
			return text + text2;
		}

		private static bool InitBundleCallStack()
		{
			int @int = PlayerPrefs.GetInt("BundleCallStack", 0);
			return @int != 0;
		}

		public static void TraceSperator()
		{
			if (Option.ms_EnableTrace)
			{
				TsLog.Log("[TsBundle] www =================================================================", new object[0]);
			}
		}

		private static bool InitBundleTrace()
		{
			int @int = PlayerPrefs.GetInt("BundleTrace", 0);
			return @int != 0;
		}

		private static bool InitPreDownload()
		{
			int @int = PlayerPrefs.GetInt(Option.KEY_PREDOWNLOAD, 1);
			return @int != 0;
		}

		private static bool InitPreDownloadHistory()
		{
			int @int = PlayerPrefs.GetInt(Option.KEY_PREDOWNLOAD_HISTORY, 0);
			return @int != 0;
		}

		private static float InitDownloadDelay()
		{
			return PlayerPrefs.GetFloat("DownloadDelay", 0.5f);
		}

		private static bool InitVerifyLoadedBundle()
		{
			return PlayerPrefs.GetInt("VerifyLoadedBundle", 0) == 1;
		}

		public static void SetPause(bool onOff)
		{
			if (Option.ms_pause != onOff)
			{
				TsLog.LogWarning("TsBundle Service Pause {0}", new object[]
				{
					onOff
				});
			}
			Option.ms_pause = onOff;
		}
	}
}
