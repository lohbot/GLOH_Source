using SERVICE;
using System;
using TsBundle;

public class NkServiceAreaInfo
{
	public eSERVICE_AREA eServiceArea = eSERVICE_AREA.SERVICE_ANDROID_KORLOCAL;

	public string szPrefsKey = NrPrefsKey.INTERNET_WWW_PATH;

	public Protocol eProtocolType;

	public string szServiceKey = "SERVICE_ANDROID_KORLOCAL";

	public string szOriginalDataCDNPath = "/D:/ndoors/at2dev/Mobile/";

	public string szEdgeDataCDNPath = "/D:/ndoors/at2dev/Mobile/";

	public string[] szLoginIP = new string[3];

	public string szWebDomain = "klohw.ndoors.com";

	public string szPrivateIP = "20.0.1.18";

	public string szPrivateDomain = "klohw.ndoors.com";

	public string szImageURL = "lohweb.s3-ap-northeast-1.amazonaws.com/klohw";

	public NkServiceAreaInfo(eSERVICE_AREA sa, string sk)
	{
		this.eServiceArea = sa;
		this.szServiceKey = sk;
		for (int i = 0; i < 3; i++)
		{
			if (this.szLoginIP[i] == null)
			{
				this.szLoginIP[i] = string.Empty;
			}
			this.szLoginIP[i] = "20.0.1.18";
		}
	}

	public void SetOriginalDataCDNPath(string cdnpath)
	{
		this.szOriginalDataCDNPath = cdnpath;
	}

	public bool IsServiceKey(string sk)
	{
		return this.szServiceKey.Equals(sk);
	}
}
