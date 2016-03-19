using System;
using TsBundle;
using UnityEngine;

public class EventTriggerSceneBundle : MonoBehaviour
{
	public string m_SceneName;

	public string m_URL;

	public string m_AssetPath;

	public bool m_DownComplete;

	public bool m_LoadComplete;

	public bool m_LoadTest;

	public void OpenAssetScene(string AssetPath, string SceneName)
	{
		if (EventTriggerEditorUtil._OnOpenSceneAsset != null)
		{
			EventTriggerEditorUtil._OnOpenSceneAsset(AssetPath);
			this.m_LoadComplete = true;
		}
		this.m_AssetPath = AssetPath;
		this.m_SceneName = SceneName;
		this.m_DownComplete = true;
	}

	public void SetSceneBundleInfo(string URL, string AssetPath, string SceneName, bool immediatelyUrl)
	{
		this.m_URL = URL;
		this.m_AssetPath = AssetPath;
		this.m_SceneName = SceneName;
		TsLog.Log(string.Format("[SetSceneBundleInfo] SceneName:{0}, AssetPath:{1}", SceneName, AssetPath), new object[0]);
		if (!string.IsNullOrEmpty(this.m_URL))
		{
			WWWItem wWWItem = Holder.TryGetOrCreateBundle(this.m_URL, null, immediatelyUrl);
			wWWItem.SetItemType(ItemType.USER_ASSETB);
			wWWItem.SetCallback(new PostProcPerItem(this._OnCompleteDownload), null);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
		}
		else
		{
			this.m_DownComplete = true;
		}
	}

	private void _OnCompleteDownload(IDownloadedItem wItem, object obj)
	{
		if (wItem.canAccessAssetBundle)
		{
			if (Application.CanStreamedLevelBeLoaded(this.m_SceneName))
			{
				if (!this.m_LoadTest)
				{
					Application.LoadLevelAdditive(this.m_SceneName);
				}
				this.m_DownComplete = true;
				this.m_LoadComplete = true;
				return;
			}
			if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
			{
				TsLog.Log("Can't StreamedLevelBeLoaded:" + this.m_SceneName, new object[0]);
			}
		}
		this.m_DownComplete = true;
		this.m_LoadComplete = false;
	}
}
