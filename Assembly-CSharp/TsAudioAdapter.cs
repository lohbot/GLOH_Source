using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class TsAudioAdapter : MonoBehaviour
{
	public EAudioType AudioType
	{
		get
		{
			return this.GetAudioEx().baseData.AudioType;
		}
	}

	public bool IsAppendGUIInfo
	{
		get;
		set;
	}

	public TsAudio GetAudioEx()
	{
		TsAudio tsAudio = this._GetAudioEx();
		tsAudio.CheckAndSetNeedRefs(this);
		return tsAudio;
	}

	protected abstract TsAudio _GetAudioEx();

	public abstract bool _InitAudioEx(TsAudio audioEx);

	[ContextMenu("Fill BundleInfo")]
	public bool FillBundleInfo()
	{
		this.GetAudioEx().RefAudioSource = base.audio;
		if (base.audio.clip != null)
		{
			this.GetAudioEx().RefAudioClip = base.audio.clip;
		}
		return this.GetAudioEx().FillBundleInfo();
	}

	[ContextMenu("Fill BundleInfo And Set AudioClip = null")]
	public bool FillBundleInfoAndSetAudioClipNull()
	{
		if (!this.FillBundleInfo())
		{
			return false;
		}
		this.GetAudioEx().RefAudioClip = null;
		return true;
	}

	[ContextMenu("-  Add BundleInfo")]
	public void AddBundleInfo()
	{
		this.GetAudioEx().AddBundleInfo();
	}

	[ContextMenu("- Remove Last BundleInfo")]
	public void RemoveBundleInfo(ref int nIndex)
	{
		this.GetAudioEx().RemoveBundleInfo(ref nIndex);
	}

	public void UpBundleInfo(ref int nIndex)
	{
		this.GetAudioEx().UpBundleInfo(ref nIndex);
	}

	public void DownBundleInfo(ref int nIndex)
	{
		this.GetAudioEx().DownBundleInfo(ref nIndex);
	}

	public static TsAudioAdapter TryToAddAdapter(GameObject go, TsAudio audioEx)
	{
		if (go == null)
		{
			return null;
		}
		TsAudioAdapter tsAudioAdapter = go.GetComponent<TsAudioAdapter>();
		if (tsAudioAdapter != null && tsAudioAdapter.AudioType != audioEx.baseData.AudioType)
		{
			UnityEngine.Object.DestroyImmediate(tsAudioAdapter);
			tsAudioAdapter = null;
		}
		if (tsAudioAdapter == null)
		{
			if (go.audio != null)
			{
				go.audio.playOnAwake = false;
			}
			switch (audioEx.baseData.AudioType)
			{
			case EAudioType.SFX:
				tsAudioAdapter = go.AddComponent<TsAudioAdapterSFX>();
				break;
			case EAudioType.BGM:
				tsAudioAdapter = go.AddComponent<TsAudioAdapterBGM>();
				break;
			case EAudioType.AMBIENT:
				tsAudioAdapter = go.AddComponent<TsAudioAdapterAmbient>();
				break;
			case EAudioType.UI:
				tsAudioAdapter = go.AddComponent<TsAudioAdapterUI>();
				break;
			case EAudioType.VOICE:
				tsAudioAdapter = go.AddComponent<TsAudioAdapterVoice>();
				break;
			case EAudioType.SYSTEM:
				tsAudioAdapter = go.AddComponent<TsAudioAdapterSystem>();
				break;
			case EAudioType.GAME_DRAMA:
				tsAudioAdapter = go.AddComponent<TsAudioAdapterGameDrama>();
				break;
			case EAudioType.ENVIRONMENT:
				tsAudioAdapter = go.AddComponent<TsAudioAdapterEnvironment>();
				break;
			default:
				TsLog.Assert(false, "Check the AudioType~!!! Invalid Type= {0}", new object[]
				{
					audioEx.baseData.AudioType
				});
				break;
			}
		}
		if (tsAudioAdapter == null)
		{
			TsLog.LogError("Check the EAudioType~! is Invaild Value EAudioType = " + audioEx.baseData.AudioType, new object[0]);
			return null;
		}
		if (!tsAudioAdapter._InitAudioEx(audioEx))
		{
			UnityEngine.Object.Destroy(tsAudioAdapter);
			return null;
		}
		audioEx.CheckAndSetNeedRefs(tsAudioAdapter);
		return tsAudioAdapter;
	}

	public void Awake()
	{
		if (base.gameObject.activeInHierarchy && base.audio != null && base.audio.clip != null && base.audio.playOnAwake && !TsAudio.IsMuteAudioType(this.AudioType))
		{
			if (this.GetAudioEx().baseData.SkipIfPlayingSame)
			{
				if (TsAudioPlayingList.isPlaying(base.audio.clip.name))
				{
					base.audio.playOnAwake = false;
					base.audio.Stop();
				}
				else
				{
					TsAudioPlayingList.Add(base.audio.clip.name, null);
					this.DestroyAfter(base.audio.clip.length);
				}
			}
		}
		else
		{
			this.GetAudioEx().OnAwake(this);
		}
	}

	public void Update()
	{
		this.GetAudioEx().Update();
		this.ChildUpdate();
	}

	public virtual void ChildUpdate()
	{
	}

	public void OnDestroy()
	{
		this.GetAudioEx().OnDestroy();
	}

	public void OnDrawGizmos()
	{
		this.GetAudioEx().DrawGizmos(base.gameObject);
	}

	public void Play()
	{
		this.GetAudioEx().Play();
	}

	public void PlayOneShot()
	{
		this.GetAudioEx().PlayOneShot();
	}

	public void PlayClipAtPoint(Vector3 playPoint)
	{
		this.GetAudioEx().PlayClipAtPoint(playPoint);
	}

	public void DestroyAfter(float sec)
	{
		base.Invoke("SelfDestroy", sec);
	}

	private void SelfDestroy()
	{
		if (base.gameObject.transform.parent == BugFixAudio.PlayOnceRoot.transform)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			TsAudioPlayingList.Remove(base.audio.clip.name);
		}
	}

	public override string ToString()
	{
		return this.GetAudioEx().ToString();
	}
}
