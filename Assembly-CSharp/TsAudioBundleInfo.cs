using System;
using System.IO;
using TsBundle;
using UnityEngine;

[Serializable]
public class TsAudioBundleInfo
{
	public delegate string OnEvent_GetAssetPath(AudioClip audioClip);

	public delegate AudioClip OnEvent_LoadAssetAtPath(string assetPath);

	private static string ms_LocalPath;

	private static string ms_SoundFolder = "Sound";

	private bool m_bMuteAudio;

	public static readonly string Ext = ".assetbundle";

	[SerializeField]
	private string m_assetBundleName = string.Empty;

	[SerializeField]
	private string m_assetPathOfAudioClip = string.Empty;

	[SerializeField]
	private string m_audioClipName = string.Empty;

	[SerializeField]
	private bool _isIgnoreRandomMode;

	public static TsAudioBundleInfo.OnEvent_GetAssetPath EventGetAssetPath;

	public static TsAudioBundleInfo.OnEvent_LoadAssetAtPath EventLoadAssetAtPath;

	public static string BasePath
	{
		get
		{
			if (NrTSingleton<NrGlobalReference>.Instance.useCache && TsPlatform.IsMobile && !TsPlatform.IsEditor)
			{
				return "sound/";
			}
			return "Sound/";
		}
	}

	public string AssetPathOfAudioClip
	{
		get
		{
			return this.m_assetPathOfAudioClip;
		}
		set
		{
			this.m_assetPathOfAudioClip = value;
		}
	}

	public string AssetBundleName
	{
		get
		{
			return this.m_assetBundleName + TsAudioBundleInfo.Ext;
		}
		set
		{
			this.m_assetBundleName = value;
		}
	}

	public string AssetBundleNameWithoutExtension
	{
		get
		{
			return this.m_assetBundleName;
		}
	}

	public string AudioClipName
	{
		get
		{
			if (string.IsNullOrEmpty(this.m_audioClipName))
			{
				this.m_audioClipName = Path.GetFileNameWithoutExtension(this.AssetBundleName);
			}
			return this.m_audioClipName;
		}
		set
		{
			this.m_audioClipName = value;
		}
	}

	public string LocalBundlePath
	{
		get
		{
			if (TsAudioBundleInfo.ms_LocalPath == null)
			{
				TsAudioBundleInfo.ms_LocalPath = Application.dataPath + "/../BUNDLE/sound/";
			}
			return TsAudioBundleInfo.ms_LocalPath + this.AssetBundleName;
		}
	}

	public string DownloadPath
	{
		get
		{
			return TsAudioBundleInfo.BasePath + this.AssetBundleName;
		}
	}

	public bool IsValid
	{
		get
		{
			return !string.IsNullOrEmpty(this.m_assetBundleName) && !string.IsNullOrEmpty(this.m_audioClipName) && !string.IsNullOrEmpty(this.m_assetPathOfAudioClip);
		}
	}

	public bool IsRequestDownloaded
	{
		get;
		private set;
	}

	public bool IsIgnoreRandomMode
	{
		get
		{
			return this._isIgnoreRandomMode;
		}
		set
		{
			this._isIgnoreRandomMode = value;
		}
	}

	public static string GetDownloadPath(string bundleName)
	{
		return TsAudioBundleInfo.BasePath + bundleName + TsAudioBundleInfo.Ext;
	}

	public override string ToString()
	{
		return string.Format("BundleName({0}) AssetPath({1})", this.AssetBundleName, this.AssetPathOfAudioClip);
	}

	public bool FillBundleInfo(AudioClip audioClip)
	{
		if (audioClip == null)
		{
			return false;
		}
		this.m_assetPathOfAudioClip = this.GetAssetPathOfAudioClip(audioClip);
		this.m_assetBundleName = TsAudioBundleInfo._MakeAssetBundleName(this.m_assetPathOfAudioClip);
		this.m_audioClipName = audioClip.name;
		return true;
	}

	public bool FillBundleInfo(string audioClipName, string assetPath, string assetBundleName)
	{
		if (string.IsNullOrEmpty(audioClipName) || string.IsNullOrEmpty(assetPath) || string.IsNullOrEmpty(assetBundleName))
		{
			return false;
		}
		this.m_assetPathOfAudioClip = assetPath;
		this.m_assetBundleName = assetBundleName;
		this.m_audioClipName = audioClipName;
		return true;
	}

	public string GetAssetPathOfAudioClip(AudioClip audioClip)
	{
		if (!Application.isEditor)
		{
			TsLog.LogError("Cannot use this Function~! only EditorMode~!", new object[0]);
			return null;
		}
		if (TsAudioBundleInfo.EventGetAssetPath == null)
		{
			return this.m_assetPathOfAudioClip;
		}
		return TsAudioBundleInfo.EventGetAssetPath(audioClip);
	}

	public AudioClip LoadAssetAtPath()
	{
		if (!Application.isEditor)
		{
			TsLog.LogError("Cannot use this Function~! only EditorMode~!", new object[0]);
			return null;
		}
		if (TsAudioBundleInfo.EventLoadAssetAtPath == null)
		{
			return null;
		}
		return TsAudioBundleInfo.EventLoadAssetAtPath(this.m_assetPathOfAudioClip);
	}

	public static string _MakeAssetBundleName(string assetPathOfAudioClip)
	{
		if (assetPathOfAudioClip.Length <= 0)
		{
			return string.Empty;
		}
		string text = string.Empty;
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(assetPathOfAudioClip);
		assetPathOfAudioClip = Path.GetDirectoryName(assetPathOfAudioClip);
		string[] array = assetPathOfAudioClip.Split(new char[]
		{
			'\\',
			'/'
		});
		bool flag = false;
		for (int i = 0; i < array.Length; i++)
		{
			string text2 = array[i];
			if (text2.Equals(TsAudioBundleInfo.ms_SoundFolder, StringComparison.CurrentCultureIgnoreCase))
			{
				flag = true;
			}
			else if (flag)
			{
				text += text2;
				if (i != array.Length)
				{
					text += '/';
				}
			}
		}
		return text + fileNameWithoutExtension;
	}

	public bool _RequestDownload(TsAudio.RequestData requestData, PostProcPerItem onEvent)
	{
		if (string.IsNullOrEmpty(this.m_assetBundleName))
		{
			return false;
		}
		if (onEvent == null)
		{
			return false;
		}
		if (TsAudio.IsDisableDownloadAudio(requestData.baseData.AudioType))
		{
			return true;
		}
		if (TsAudio.IsDisableDownloadByTag(requestData.baseData.Tag))
		{
			return true;
		}
		bool unloadAfterPostProcess = false;
		string stackName;
		if (requestData.baseData.IsDontDestroyOnLoad)
		{
			stackName = Option.IndependentFromStageStackName;
			unloadAfterPostProcess = true;
		}
		else
		{
			stackName = TsAudio.AssetBundleStackName;
			if (requestData.baseData.AudioType == EAudioType.BGM || requestData.baseData.AudioType == EAudioType.AMBIENT || requestData.baseData.AudioType == EAudioType.ENVIRONMENT)
			{
				unloadAfterPostProcess = true;
			}
		}
		if (!this.m_bMuteAudio)
		{
			string text = string.Empty;
			if (requestData.baseData.AudioType == EAudioType.BGM_STREAM)
			{
				text = this.m_assetPathOfAudioClip.Replace("Assets/", string.Empty);
				text = text.Replace("wav", "mp3");
			}
			else
			{
				text = string.Format("{0}_mobile{1}", this.DownloadPath.Replace(TsAudioBundleInfo.Ext, string.Empty), TsAudioBundleInfo.Ext);
			}
			WWWItem wWWItem = Holder.TryGetOrCreateBundle(text, stackName);
			if (requestData.baseData.AudioType == EAudioType.BGM_STREAM)
			{
				wWWItem.SetItemType(ItemType.USER_AUDIO);
			}
			else
			{
				wWWItem.SetItemType(ItemType.AUDIO);
			}
			wWWItem.SetCallback(onEvent, requestData);
			wWWItem.SetLoadAll(true);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, unloadAfterPostProcess);
			this.IsRequestDownloaded = true;
		}
		return true;
	}

	public void RemoveUsedWWWItem()
	{
		if (!this.IsRequestDownloaded)
		{
			return;
		}
	}
}
