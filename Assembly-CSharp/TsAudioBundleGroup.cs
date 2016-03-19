using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;

[AddComponentMenu("Audio/Ndoors/Additional/Ts Audio Bundle Group")]
public class TsAudioBundleGroup : MonoBehaviour
{
	[SerializeField]
	private List<TsAudio.BaseData> _audioBaseDatas = new List<TsAudio.BaseData>();

	private List<TsAudio> _downloadedAudioEx = new List<TsAudio>();

	public bool IsAllAudioClipsDownloaded
	{
		get
		{
			return this._audioBaseDatas.Count == this._downloadedAudioEx.Count;
		}
	}

	public TsAudio.BaseData[] AudioBaseDatas
	{
		get
		{
			return this._audioBaseDatas.ToArray();
		}
	}

	public List<TsAudio.BaseData> AudioBaseDatasTemp
	{
		get
		{
			return this._audioBaseDatas;
		}
		set
		{
			this._audioBaseDatas = value;
		}
	}

	public void Start()
	{
		for (int i = 0; i < this._audioBaseDatas.Count; i++)
		{
			TsAudio.BaseData baseData = this._audioBaseDatas[i];
			if (baseData.DefaultBundleInfo._RequestDownload(new TsAudio.RequestData(i, baseData), new PostProcPerItem(this.OnEvent_Downloaded)))
			{
				baseData.RequestedIndexList.Add(i);
			}
		}
	}

	public TsAudio GetAudioEx(int index)
	{
		if (this.IsAllAudioClipsDownloaded)
		{
			TsAudio tsAudio = null;
			try
			{
				tsAudio = this._downloadedAudioEx[index];
			}
			catch (Exception ex)
			{
				TsLog.LogWarning("GetAudioEx( int index )  Exception= " + ex.ToString(), new object[0]);
			}
			if (tsAudio != null && tsAudio.RefAudioClip != null)
			{
				return tsAudio;
			}
		}
		return null;
	}

	public TsAudio GetAudioEx(string audioClipName)
	{
		if (this.IsAllAudioClipsDownloaded)
		{
			foreach (TsAudio current in this._downloadedAudioEx)
			{
				if (!(current.RefAudioClip == null))
				{
					if (current.baseData.DefaultBundleInfo.AudioClipName.Equals(audioClipName, StringComparison.CurrentCultureIgnoreCase))
					{
						return current;
					}
				}
			}
		}
		return null;
	}

	public AudioClip GetAudioClipFromAssetPath(string audioClipName)
	{
		foreach (TsAudio.BaseData current in this._audioBaseDatas)
		{
			if (current.DefaultBundleInfo.AudioClipName.Equals(audioClipName))
			{
				return current.DefaultBundleInfo.LoadAssetAtPath();
			}
		}
		return null;
	}

	public void OnEvent_Downloaded(IDownloadedItem wItem, object obj)
	{
		if (!wItem.canAccessAssetBundle)
		{
			return;
		}
		AudioClip x = wItem.mainAsset as AudioClip;
		if (x == null)
		{
			TsLog.LogError("Error! It's not AudioClip. DownLoadPath= " + wItem.assetPath, new object[0]);
			return;
		}
		TsAudio.RequestData requestData = obj as TsAudio.RequestData;
		TsAudio tsAudio = TsAudioCreator.Create(requestData.baseData);
		if (tsAudio == null)
		{
			TsLog.LogError("Error! Cannot Create TsAudio DownLoadPath= " + wItem.assetPath, new object[0]);
			return;
		}
		tsAudio.RefAudioClip = (wItem.mainAsset as AudioClip);
		TsLog.Log("OnEvent_Downloaded() Success Download~! ClipName= " + tsAudio.RefAudioClip.name, new object[0]);
		this._downloadedAudioEx.Add(tsAudio);
		wItem.unloadImmediate = true;
	}

	public void OnClickedAdd(AudioClip audioClip, EAudioType audioType)
	{
		if (audioClip == null)
		{
			return;
		}
		foreach (TsAudio.BaseData current in this._audioBaseDatas)
		{
			if (current.DefaultBundleInfo.AudioClipName.Equals(audioClip.name, StringComparison.CurrentCultureIgnoreCase))
			{
				TsLog.Log("Failed~! Already Contained~!", new object[0]);
				return;
			}
		}
		TsAudio.BaseData baseData = TsAudio.BaseData.Create(audioType);
		baseData.DefaultBundleInfo.FillBundleInfo(audioClip);
		TsLog.Log("OnClickedAdd() Info= " + baseData.DefaultBundleInfo.ToString(), new object[0]);
		this._audioBaseDatas.Add(baseData);
	}

	public void OnClickedRemove(string audioClipName)
	{
		if (string.IsNullOrEmpty(audioClipName))
		{
			return;
		}
		if (this._audioBaseDatas.Count <= 0)
		{
			return;
		}
		TsAudio.BaseData item = null;
		foreach (TsAudio.BaseData current in this._audioBaseDatas)
		{
			if (current.DefaultBundleInfo.AudioClipName.Equals(audioClipName))
			{
				item = current;
				break;
			}
		}
		this._audioBaseDatas.Remove(item);
	}
}
