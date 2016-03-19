using NLibCs;
using SERVICE;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TsBundle;
using UnityEngine;

public class NrGlobalReference : NrTSingleton<NrGlobalReference>
{
	public class DownloadInfo
	{
		public int m_si32SceneSizeDownloaded;

		public int m_si32TotalSizeDownloaded;

		public void ResetSceneDownloadSize()
		{
			this.m_si32SceneSizeDownloaded = 0;
		}
	}

	public static string strLangType = "kor";

	public static string strServiceCode = eSERVICE_AREA.SERVICE_ANDROID_KORLOCAL.ToString();

	public static string MobileIDAdd = "com.nexon.loh.";

	public static string strMobileIdentifier = "com.nexon.loh.korlocal";

	public static string strLoginServerIP = "20.0.1.18";

	public static string strWebPageDomain = "klohw.ndoors.com";

	public static string strUSAWepPageDomain = "nexonm1.zendesk.com";

	public static string strPackageSuffix = string.Empty;

	public static string strLiteCheck = "off";

	private NrGlobalReference.DownloadInfo m_kDownloadInfo = new NrGlobalReference.DownloadInfo();

	private NkServiceAreaInfo m_kCurrentServiceAreaInfo;

	private List<NkServiceAreaInfo> m_kServiceAreaInfoList = new List<NkServiceAreaInfo>();

	private bool m_bSetServiceArea;

	private string strMobileVer = string.Empty;

	private static string strCDNType = "korea";

	private static string strCDNTypeUS = "cloud";

	private string strResourcesVer = string.Empty;

	private string strPublicMode = "yes";

	private static bool _isWebBasepathExteranlCalled = false;

	public bool IsEnableLog
	{
		get;
		set;
	}

	public static string SERVICECODE
	{
		get
		{
			return NrGlobalReference.strServiceCode;
		}
		set
		{
			NrGlobalReference.strServiceCode = value;
			if (TsPlatform.IsAndroid)
			{
				string mobileCode = ServiceCode.GetMobileCode(NrGlobalReference.strServiceCode);
				NrGlobalReference.strMobileIdentifier = NrGlobalReference.MobileIDAdd + mobileCode.ToLower();
				if (!NrGlobalReference.strLiteCheck.Equals("on"))
				{
					NrGlobalReference.strMobileIdentifier += NrGlobalReference.strPackageSuffix;
				}
				else
				{
					NrGlobalReference.strMobileIdentifier += "lite";
				}
			}
			else if (TsPlatform.IsIPhone)
			{
				string mobileCode2 = ServiceCode.GetMobileCode(NrGlobalReference.strServiceCode);
				NrGlobalReference.strMobileIdentifier = NrGlobalReference.MobileIDAdd + mobileCode2.ToLower();
				if (!NrGlobalReference.strLiteCheck.Equals("on"))
				{
					NrGlobalReference.strMobileIdentifier += NrGlobalReference.strPackageSuffix;
				}
				else
				{
					NrGlobalReference.strMobileIdentifier += "lite";
				}
			}
		}
	}

	public static string MOBILEID
	{
		get
		{
			return NrGlobalReference.strMobileIdentifier;
		}
	}

	public string STR_MOBILE_VER
	{
		get
		{
			return this.strMobileVer;
		}
		set
		{
			this.strMobileVer = value;
		}
	}

	public string ResourcesVer
	{
		get
		{
			return this.strResourcesVer;
		}
		set
		{
			this.strResourcesVer = value;
		}
	}

	public string PUBLICMODE
	{
		get
		{
			return this.strPublicMode;
		}
		set
		{
			this.strPublicMode = value;
			if (this.strPublicMode.Equals("no"))
			{
				NkServiceAreaInfo currentServiceAreaInfo = this.GetCurrentServiceAreaInfo();
				if (currentServiceAreaInfo != null)
				{
					this.SetCurrentServiceAreaInfo(currentServiceAreaInfo.szServiceKey);
					UnityEngine.Debug.LogWarning("Public Mode = NO! Changed CurrentService = " + currentServiceAreaInfo.szServiceKey);
				}
			}
		}
	}

	public string basePath
	{
		get
		{
			if (this.localWWW)
			{
				return Option.GetProtocolRootPath(Protocol.FILE);
			}
			return Option.GetProtocolRootPath(Protocol.HTTP);
		}
		set
		{
			this.SetCurrentServiceAreaInfo(value);
		}
	}

	public bool localWWW
	{
		get
		{
			return Option.localWWW;
		}
		set
		{
			Option.localWWW = value;
			if (value)
			{
				PlayerPrefs.SetInt("LocalWWW", 1);
			}
		}
	}

	public bool useCache
	{
		get
		{
			return Option.IsLoadFromCacheOrDownload();
		}
		set
		{
			Option.SetLoadFromCacheOrDownload(value);
			UnityEngine.Debug.Log("NrGlobalReference.Instance.useCache = " + value);
		}
	}

	public bool usePool
	{
		get;
		set;
	}

	public bool usePackage
	{
		get;
		set;
	}

	public bool safeBundleUnload
	{
		get;
		set;
	}

	public static bool isWebBasePathExternalCalled
	{
		get
		{
			return NrGlobalReference._isWebBasepathExteranlCalled;
		}
	}

	public static string CDNTYPE
	{
		get
		{
			return NrGlobalReference.strCDNType;
		}
	}

	public static string CDNTYPEUS
	{
		get
		{
			return NrGlobalReference.strCDNTypeUS;
		}
	}

	private NrGlobalReference()
	{
		this.ReadyService();
		this.InitServiceAreaInfo();
		this.Init();
	}

	private void ReadyService()
	{
		if (!TsPlatform.IsMobile)
		{
			return;
		}
		string text = string.Empty;
		string text2 = string.Empty;
		string text3 = string.Empty;
		string text4 = string.Empty;
		string text5 = string.Empty;
		string text6 = string.Empty;
		string path = "Common/ServiceSettings/ServiceInfo";
		if (TsPlatform.IsIPhone)
		{
			path = "Common/ServiceSettings/ServiceInfo_IOS";
		}
		TextAsset textAsset = Resources.Load(path) as TextAsset;
		if (textAsset != null)
		{
			NDataReader nDataReader = new NDataReader();
			if (!nDataReader.LoadFrom(textAsset.text))
			{
				TsLog.LogError("파일 로드에 실패 하였습니다. \n{0}", new object[]
				{
					textAsset.text
				});
				return;
			}
			nDataReader.FirstLine();
			while (!nDataReader.IsEOF())
			{
				string column = nDataReader.GetCurrentRow().GetColumn(0);
				if (column.Length == 0)
				{
					nDataReader.NextLine();
				}
				else
				{
					string[] array = column.Split(new char[]
					{
						'='
					});
					if (array[0].Trim().ToLower().Equals("langtype"))
					{
						text = array[1];
						TsLog.LogWarning("langtype : {0}", new object[]
						{
							text
						});
					}
					if (array[0].Trim().ToLower().Equals("servicecode"))
					{
						text2 = array[1];
						TsLog.LogWarning("servicecode : {0}", new object[]
						{
							text2
						});
					}
					if (array[0].Trim().ToLower().Equals("apkversion"))
					{
						text3 = array[1];
						TsLog.LogWarning("apkversion : {0}", new object[]
						{
							text3
						});
					}
					if (array[0].Trim().ToLower().Equals("packagesuffix"))
					{
						text4 = array[1];
						TsLog.LogWarning("packagesuffix : {0}", new object[]
						{
							text4
						});
					}
					if (array[0].Trim().ToLower().Equals("litecheck"))
					{
						text5 = array[1];
						TsLog.LogWarning("litecheck : {0}", new object[]
						{
							text5
						});
					}
					if (array[0].Trim().ToLower().Equals("appidfix"))
					{
						text6 = array[1];
						TsLog.LogWarning("appidfix : {0}", new object[]
						{
							text6
						});
					}
					nDataReader.NextLine();
				}
			}
			if (text4.Length > 0)
			{
				NrGlobalReference.strPackageSuffix = text4.Trim().ToLower();
			}
			if (text5.Equals("on") || text5.Equals("fix"))
			{
				NrGlobalReference.strLiteCheck = text5.Trim().ToLower();
			}
			if (text.Length > 0)
			{
				NrGlobalReference.strLangType = text.Trim().ToLower();
			}
			if (text6.Length > 0)
			{
				NrGlobalReference.MobileIDAdd = text6.Trim().ToLower();
			}
			if (text2.Length > 0)
			{
				NrGlobalReference.SERVICECODE = text2.Trim();
				if (TsPlatform.IsMobile)
				{
					TsPlatform.Operator.SetMobileIdentifier();
				}
			}
			if (text3.Length > 0)
			{
				if (TsPlatform.IsAndroid)
				{
					TsPlatform.APP_VERSION_AND = text3.Trim();
				}
				else if (TsPlatform.IsIPhone)
				{
					TsPlatform.APP_VERSION_IOS = text3.Trim();
				}
			}
		}
	}

	public void Init()
	{
		this.basePath = NrGlobalReference.strServiceCode;
		if (PlayerPrefs.HasKey("EnableLog"))
		{
			this.IsEnableLog = (PlayerPrefs.GetInt("EnableLog") == 1);
		}
		else
		{
			this.IsEnableLog = false;
		}
		WWWItem.OnIncreaseSizeOfDownload += new WWWItem.OnIncreaseSizeOfDownloadHandler(this.OnIncreaseSizeOfDownload);
		this.usePool = false;
		this.usePackage = false;
		this.safeBundleUnload = false;
	}

	public void ReloginInit()
	{
		this.basePath = NrGlobalReference.strServiceCode;
		this.usePool = false;
		this.usePackage = false;
		this.safeBundleUnload = false;
	}

	public static bool IsLiteVersion()
	{
		return NrGlobalReference.strLiteCheck.Equals("on") || NrGlobalReference.strLiteCheck.Equals("fix");
	}

	public NrGlobalReference.DownloadInfo GetDownloadInfo()
	{
		return this.m_kDownloadInfo;
	}

	public void PrintCallStack()
	{
		string text = "===== call stack trace =====\n";
		StackTrace stackTrace = new StackTrace(true);
		StackFrame[] frames = stackTrace.GetFrames();
		StackFrame[] array = frames;
		for (int i = 0; i < array.Length; i++)
		{
			StackFrame stackFrame = array[i];
			string text2 = stackFrame.GetFileName() + " :: ";
			string[] array2 = text2.Split(new char[]
			{
				'\\'
			});
			string text3 = text;
			text = string.Concat(new string[]
			{
				text3,
				" + ",
				array2[array2.Length - 1],
				stackFrame.GetMethod().Name,
				"\n"
			});
		}
		UnityEngine.Debug.LogWarning(text);
	}

	public void OnIncreaseSizeOfDownload(int increaseSize)
	{
		NrTSingleton<NrGlobalReference>.Instance.GetDownloadInfo().m_si32SceneSizeDownloaded += increaseSize;
		NrTSingleton<NrGlobalReference>.Instance.GetDownloadInfo().m_si32TotalSizeDownloaded += increaseSize;
	}

	public static void SetWebBasePathExternalCalled()
	{
		NrGlobalReference._isWebBasepathExteranlCalled = true;
	}

	public eSERVICE_AREA GetCurrentServiceArea()
	{
		if (this.m_kCurrentServiceAreaInfo == null)
		{
			return eSERVICE_AREA.SERVICE_ANDROID_KORLOCAL;
		}
		return this.m_kCurrentServiceAreaInfo.eServiceArea;
	}

	public static bool IsServiceArea(eSERVICE_AREA servicearea)
	{
		return NrGlobalReference.strServiceCode.Equals(servicearea.ToString());
	}

	public bool IsLocalServiceArea()
	{
		return this.m_kCurrentServiceAreaInfo == null || NrGlobalReference.SERVICECODE.Contains("LOCAL");
	}

	private void SetServiceAreaInfo(ref NkServiceAreaInfo pkServiceAreaInfo)
	{
		string text = pkServiceAreaInfo.szServiceKey;
		string path = string.Format("Common/ServiceSettings/ServiceSetting_{0}", NrGlobalReference.strLangType);
		TextAsset textAsset = Resources.Load(path) as TextAsset;
		if (textAsset == null)
		{
			TsLog.LogError("serviceFileAsset != null", new object[0]);
			return;
		}
		NDataReader nDataReader = new NDataReader();
		if (!nDataReader.LoadFrom(textAsset.text))
		{
			TsLog.LogError("파일 로드에 실패 하였습니다. \n{0}", new object[]
			{
				textAsset.text
			});
			return;
		}
		string text2 = string.Empty;
		bool flag = false;
		if (NrGlobalReference.strLiteCheck.Equals("on"))
		{
			text += "_LITE";
		}
		while (!nDataReader.IsEOF())
		{
			string column = nDataReader.GetCurrentRow().GetColumn(0);
			if (column.Length == 0)
			{
				nDataReader.NextLine();
			}
			else if (column[0] == '#')
			{
				nDataReader.NextLine();
			}
			else if (column[0] == '[')
			{
				flag = column.ToLower().Equals("[" + text.ToLower() + "]");
				nDataReader.NextLine();
			}
			else if (!flag)
			{
				nDataReader.NextLine();
			}
			else
			{
				string[] array = column.Split(new char[]
				{
					'='
				});
				if (array[0].ToLower().Equals("protocol"))
				{
					text2 = array[1];
					if (text2.Equals("file"))
					{
						pkServiceAreaInfo.eProtocolType = Protocol.FILE;
						pkServiceAreaInfo.szPrefsKey = NrPrefsKey.LOCAL_WWW_PATH;
					}
					else
					{
						pkServiceAreaInfo.eProtocolType = Protocol.HTTP;
						pkServiceAreaInfo.szPrefsKey = NrPrefsKey.INTERNET_WWW_PATH;
					}
				}
				if (array[0].ToLower().Equals("originaldatacdnpath"))
				{
					pkServiceAreaInfo.szOriginalDataCDNPath = array[1];
				}
				if (array[0].ToLower().Equals("edgedatacdnpath"))
				{
					pkServiceAreaInfo.szEdgeDataCDNPath = array[1];
				}
				if (array[0].ToLower().Equals("loginip"))
				{
					pkServiceAreaInfo.szLoginIP[0] = array[1];
					pkServiceAreaInfo.szLoginIP[1] = array[1];
					pkServiceAreaInfo.szLoginIP[2] = array[1];
				}
				if (array[0].ToLower().Equals("loginip1"))
				{
					pkServiceAreaInfo.szLoginIP[0] = array[1];
				}
				if (array[0].ToLower().Equals("loginip2"))
				{
					pkServiceAreaInfo.szLoginIP[1] = array[1];
				}
				if (array[0].ToLower().Equals("loginip3"))
				{
					pkServiceAreaInfo.szLoginIP[2] = array[1];
				}
				if (array[0].ToLower().Equals("webdomain"))
				{
					pkServiceAreaInfo.szWebDomain = array[1];
				}
				if (array[0].ToLower().Equals("privateip"))
				{
					pkServiceAreaInfo.szPrivateIP = array[1];
				}
				if (array[0].ToLower().Equals("privatedomain"))
				{
					pkServiceAreaInfo.szPrivateDomain = array[1];
				}
				if (array[0].ToLower().Equals("imageurl"))
				{
					pkServiceAreaInfo.szImageURL = array[1];
				}
				nDataReader.NextLine();
			}
		}
	}

	private void AddServiceAreaInfo(eSERVICE_AREA sa)
	{
		NkServiceAreaInfo nkServiceAreaInfo = new NkServiceAreaInfo(sa, sa.ToString());
		this.SetServiceAreaInfo(ref nkServiceAreaInfo);
		if (!TsPlatform.IsEditor)
		{
			switch (sa)
			{
			case eSERVICE_AREA.SERVICE_ANDROID_GLOBALENGLOCAL:
			case eSERVICE_AREA.SERVICE_ANDROID_GLOBALJPNLOCAL:
			case eSERVICE_AREA.SERVICE_ANDROID_GLOBALCHNLOCAL:
			case eSERVICE_AREA.SERVICE_ANDROID_GLOBALTESTLOCAL:
			case eSERVICE_AREA.SERVICE_ANDROID_USLOCAL:
				goto IL_77;
			case eSERVICE_AREA.SERVICE_ANDROID_GLOBALENGQA:
			case eSERVICE_AREA.SERVICE_ANDROID_GLOBALJPNQA:
			case eSERVICE_AREA.SERVICE_ANDROID_GLOBALCHNQA:
			case eSERVICE_AREA.SERVICE_ANDROID_GLOBALTESTQA:
				IL_53:
				if (sa != eSERVICE_AREA.SERVICE_ANDROID_KORLOCAL && sa != eSERVICE_AREA.SERVICE_ANDROID_CNLOCAL && sa != eSERVICE_AREA.SERVICE_ANDROID_JPLOCAL && sa != eSERVICE_AREA.SERVICE_IOS_KORLOCAL)
				{
					goto IL_96;
				}
				goto IL_77;
			}
			goto IL_53;
			IL_77:
			nkServiceAreaInfo.SetOriginalDataCDNPath(TsPlatform.Operator.GetFileDir() + "/");
		}
		IL_96:
		this.m_kServiceAreaInfoList.Add(nkServiceAreaInfo);
	}

	private void InitServiceAreaInfo()
	{
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_ANDROID_KORLOCAL);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_ANDROID_KORQA);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_ANDROID_KORTSTORE);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_ANDROID_KORGOOGLE);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_ANDROID_KORNAVER);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_ANDROID_BANDNAVER);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_ANDROID_BANDGOOGLE);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_ANDROID_KORKAKAO);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_ANDROID_KAKAOTSTORE);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_ANDROID_GLOBALENGLOCAL);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_ANDROID_GLOBALENGQA);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_ANDROID_GLOBALJPNLOCAL);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_ANDROID_GLOBALJPNQA);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_ANDROID_GLOBALCHNLOCAL);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_ANDROID_GLOBALCHNQA);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_ANDROID_GLOBALTESTLOCAL);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_ANDROID_GLOBALTESTQA);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_ANDROID_USLOCAL);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_ANDROID_USQA);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_ANDROID_USGOOGLE);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_ANDROID_USAMAZON);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_ANDROID_CNLOCAL);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_ANDROID_CNQA);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_ANDROID_CNTEST);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_ANDROID_CNREVIEW);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_ANDROID_JPLOCAL);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_ANDROID_JPQA);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_ANDROID_JPQALINE);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_ANDROID_JPLINE);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_IOS_KORLOCAL);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_IOS_KORQA);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_IOS_KORAPPSTORE);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_IOS_KORKAKAO);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_IOS_USQA);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_IOS_USIOS);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_IOS_CNQA);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_IOS_JPQA);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_IOS_CNTEST);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_IOS_JPQA);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_IOS_JPQALINE);
		this.AddServiceAreaInfo(eSERVICE_AREA.SERVICE_IOS_JPLINE);
		if (!TsPlatform.IsWeb)
		{
			this.m_bSetServiceArea = true;
		}
		else
		{
			this.m_bSetServiceArea = false;
		}
	}

	public NkServiceAreaInfo GetCurrentServiceAreaInfo()
	{
		return this.m_kCurrentServiceAreaInfo;
	}

	private NkServiceAreaInfo FindServiceArea(string servicekey)
	{
		foreach (NkServiceAreaInfo current in this.m_kServiceAreaInfoList)
		{
			if (current.IsServiceKey(servicekey))
			{
				return current;
			}
		}
		return null;
	}

	private void SetCurrentServiceAreaInfo(string servicekey)
	{
		NkServiceAreaInfo nkServiceAreaInfo = this.FindServiceArea(servicekey);
		if (nkServiceAreaInfo == null)
		{
			UnityEngine.Debug.LogError("##### Game service area can't find. ###### - " + servicekey);
			if (TsPlatform.IsAndroid)
			{
				this.m_kCurrentServiceAreaInfo = this.FindServiceArea(eSERVICE_AREA.SERVICE_ANDROID_KORLOCAL.ToString());
			}
			else if (TsPlatform.IsIPhone)
			{
				this.m_kCurrentServiceAreaInfo = this.FindServiceArea(eSERVICE_AREA.SERVICE_IOS_KORLOCAL.ToString());
			}
			Option.SetProtocolRootPath(Protocol.FILE, "/D:/ndoors/at2dev/Mobile/");
			PlayerPrefs.SetString(NrPrefsKey.LOCAL_WWW_PATH, "file:///D:/ndoors/at2dev/Mobile/");
			this.localWWW = true;
			this.useCache = false;
		}
		else
		{
			this.m_kCurrentServiceAreaInfo = nkServiceAreaInfo;
			Option.SetProtocolRootPath(nkServiceAreaInfo.eProtocolType, nkServiceAreaInfo.szOriginalDataCDNPath);
			TsPlatform.FileLog("OriginalCDNPath = " + nkServiceAreaInfo.szOriginalDataCDNPath);
			if (nkServiceAreaInfo.eProtocolType == Protocol.FILE)
			{
				this.localWWW = true;
				this.useCache = false;
			}
			else
			{
				this.localWWW = false;
				this.useCache = true;
			}
			if (!PlayerPrefs.HasKey(NrPrefsKey.BASE_PATH))
			{
				PlayerPrefs.SetString(NrPrefsKey.BASE_PATH, servicekey);
			}
			if (!PlayerPrefs.HasKey(nkServiceAreaInfo.szPrefsKey))
			{
				PlayerPrefs.SetString(nkServiceAreaInfo.szPrefsKey, Option.GetProtocolRootPath(nkServiceAreaInfo.eProtocolType));
			}
			if (!this.PUBLICMODE.Equals("no"))
			{
				int num = UnityEngine.Random.Range(0, 3);
				if (num >= 3)
				{
					num = 2;
				}
				NrGlobalReference.strLoginServerIP = nkServiceAreaInfo.szLoginIP[num];
				NrGlobalReference.strWebPageDomain = nkServiceAreaInfo.szWebDomain;
			}
			else
			{
				NrGlobalReference.strLoginServerIP = nkServiceAreaInfo.szPrivateIP;
				NrGlobalReference.strWebPageDomain = nkServiceAreaInfo.szPrivateDomain;
				NkServiceAreaInfo expr_186 = nkServiceAreaInfo;
				expr_186.szImageURL += "de";
			}
			TsLog.LogWarning("LoginServerIP : {0}, WebPageDomain : {1}", new object[]
			{
				NrGlobalReference.strLoginServerIP,
				NrGlobalReference.strWebPageDomain
			});
		}
		UnityEngine.Debug.LogWarning(" ##### Changed service area to " + this.m_kCurrentServiceAreaInfo.eServiceArea.ToString() + " ##### ");
		this.m_bSetServiceArea = true;
	}

	public bool IsSetServiceArea()
	{
		return this.m_bSetServiceArea;
	}

	public void ChangeEdgeDataCDNPath()
	{
		Option.SetProtocolRootPath(this.m_kCurrentServiceAreaInfo.eProtocolType, this.m_kCurrentServiceAreaInfo.szEdgeDataCDNPath);
		TsPlatform.FileLog("EdgeCDNPath = " + this.m_kCurrentServiceAreaInfo.szEdgeDataCDNPath);
	}
}
