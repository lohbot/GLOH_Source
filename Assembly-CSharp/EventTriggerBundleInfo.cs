using System;
using TsBundle;
using UnityEngine;

[Serializable]
public class EventTriggerBundleInfo
{
	public string m_AssetPath;

	public string m_BundleName;

	public UnityEngine.Object m_Object;

	public string m_URL;

	public bool m_DownComplete;

	public bool m_LoadComplete;

	[HideInInspector]
	public bool UnLoad = true;

	public EventTriggerBundleInfo(string AssetPath, string BundleName, string url)
	{
		this.m_AssetPath = AssetPath;
		this.m_BundleName = BundleName;
		this.m_URL = url;
		if (string.IsNullOrEmpty(this.m_URL))
		{
			this.m_DownComplete = true;
		}
	}

	public bool IsKey(string AssetPath)
	{
		return this.m_AssetPath == AssetPath;
	}

	public void CreateBundle()
	{
		if (!string.IsNullOrEmpty(this.m_URL))
		{
			WWWItem wWWItem = Holder.TryGetOrCreateBundle(this.m_URL, null);
			wWWItem.SetItemType(ItemType.USER_ASSETB);
			wWWItem.SetCallback(new PostProcPerItem(this._OnCompleteDownload), null);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, this.UnLoad);
		}
		else
		{
			if (EventTriggerEditorUtil._OnGetAsset != null)
			{
				this.m_Object = EventTriggerEditorUtil._OnGetAsset(this.m_AssetPath, typeof(UnityEngine.Object));
				if (this.m_Object != null)
				{
					this.m_LoadComplete = true;
				}
			}
			this.m_DownComplete = true;
		}
	}

	private void _OnCompleteDownload(IDownloadedItem wItem, object obj)
	{
		if (wItem.canAccessAssetBundle && wItem.GetSafeBundle().mainAsset)
		{
			wItem.GetSafeBundle().LoadAll();
			this.m_Object = wItem.GetSafeBundle().mainAsset;
			this.m_LoadComplete = true;
		}
		this.m_DownComplete = true;
	}

	public UnityEngine.Object GetObject()
	{
		if (!(this.m_Object != null))
		{
			if (this.m_LoadComplete)
			{
				return this.m_Object;
			}
			if (EventTriggerEditorUtil._OnGetAsset != null)
			{
				this.m_Object = EventTriggerEditorUtil._OnGetAsset(this.m_AssetPath, typeof(UnityEngine.Object));
			}
		}
		return this.m_Object;
	}
}
