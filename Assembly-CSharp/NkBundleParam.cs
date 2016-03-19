using System;
using TsBundle;

public class NkBundleParam
{
	public enum eBundleType
	{
		BUNDLE_NONE,
		BUNDLE_CHAR_BONE,
		BUNDLE_CHAR_BONEPRELOAD,
		BUNDLE_CHAR_ANIMATION,
		BUNDLE_CHAR_SWITCHPART,
		BUNDLE_CHAR_ATTACHPART,
		BUNDLE_CHAR_ATTACHITEM,
		BUNDLE_CHAR_RIDE,
		BUNDLE_CHAR_NONEPART,
		BUNDLE_CHAR_OBJECT,
		BUNDLE_UI_DIALOG,
		BUNDLE_NORMAL_CALLBACK,
		BUNDLE_OBJECTPARAM_CALLBACK,
		MAX_BUNDLE_NUM
	}

	public delegate void funcBundleCallBack(ref IDownloadedItem wItem);

	public delegate void funcParamBundleCallBack(ref IDownloadedItem IDownloadedItem, object paramobj);

	private NkBundleParam.eBundleType m_kBundleType;

	private string m_szBundleKey = string.Empty;

	private string m_szCategory = string.Empty;

	private bool m_bPreload;

	private object m_kParamData;

	private int m_nParam;

	private string m_szParam = string.Empty;

	public NkBundleParam.funcBundleCallBack funcCallBack
	{
		get;
		set;
	}

	public NkBundleParam.funcParamBundleCallBack funcParamCallBack
	{
		get;
		set;
	}

	public NkBundleParam(NkBundleParam.eBundleType bundletype, string key)
	{
		this.m_kBundleType = bundletype;
		this.m_szBundleKey = key;
		this.m_szCategory = string.Empty;
		this.m_bPreload = false;
		this.m_kParamData = null;
	}

	public NkBundleParam.eBundleType GetBundleType()
	{
		return this.m_kBundleType;
	}

	public string GetBundleKey()
	{
		return this.m_szBundleKey;
	}

	public void SetCategory(string category)
	{
		this.m_szCategory = category;
	}

	public string GetCategory()
	{
		return this.m_szCategory;
	}

	public void SetPreload(bool preload)
	{
		this.m_bPreload = preload;
	}

	public bool IsPreload()
	{
		return this.m_bPreload;
	}

	public void SetParamObject(object obj)
	{
		this.m_kParamData = obj;
	}

	public object GetParamObject()
	{
		return this.m_kParamData;
	}

	public void SetParam(int num, string str)
	{
		this.m_nParam = num;
		this.m_szParam = str;
	}

	public int GetNumParam()
	{
		return this.m_nParam;
	}

	public string GetStrParam()
	{
		return this.m_szParam;
	}
}
