using System;
using UnityEngine;

public class Nr3DCharItemAssetBundle
{
	private AssetBundle m_kAssetBundle;

	private string m_szTargetName = string.Empty;

	private Vector3 m_vScale = Vector3.zero;

	private bool m_bSetBundle;

	private string m_szLoadItemName = string.Empty;

	public Nr3DCharItemAssetBundle()
	{
		this.Init();
	}

	public void Init()
	{
		this.InitBundleInfo();
		this.m_szLoadItemName = string.Empty;
	}

	public void InitBundleInfo()
	{
		this.m_kAssetBundle = null;
		this.m_szTargetName = string.Empty;
		this.m_vScale = Vector3.zero;
		this.m_bSetBundle = false;
	}

	public bool IsValid()
	{
		return this.m_bSetBundle;
	}

	public void SetAssetBundle(AssetBundle kAssetBundle)
	{
		this.m_kAssetBundle = kAssetBundle;
		this.m_bSetBundle = true;
	}

	public AssetBundle GetAssetBundle()
	{
		return this.m_kAssetBundle;
	}

	public void SetTargetName(string targetname)
	{
		this.m_szTargetName = targetname;
	}

	public string GetTargetName()
	{
		return this.m_szTargetName;
	}

	public void SetScale(Vector3 vScale)
	{
		this.m_vScale = vScale;
	}

	public Vector3 GetScale()
	{
		return this.m_vScale;
	}

	public void SetLoadItemName(string itemname)
	{
		this.m_szLoadItemName = itemname;
		this.m_bSetBundle = false;
	}

	public string GetLoadItemName()
	{
		return this.m_szLoadItemName;
	}

	public bool IsSameLoadItemName(string itemname)
	{
		return this.m_szLoadItemName.Equals(itemname) && this.IsValid();
	}

	public bool IsLoadedBundle()
	{
		return this.m_szLoadItemName.Length > 0;
	}
}
