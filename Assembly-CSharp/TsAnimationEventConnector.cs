using System;
using TsBundle;
using UnityEngine;

[AddComponentMenu("Animation/Ndoors/Ts Animation Event Connector")]
public class TsAnimationEventConnector : MonoBehaviour
{
	private FX_MS_TrailArc _TrailEffect;

	private AudioSource audioSrc;

	public void Start()
	{
		if (base.audio != null)
		{
			this.audioSrc = base.audio;
		}
		else
		{
			GameObject gameObject = BugFixAudio.NewBugFixAudioSource(base.transform);
			if (gameObject != null)
			{
				gameObject.transform.parent = base.transform;
				this.audioSrc = gameObject.audio;
			}
		}
	}

	public virtual void OnEvent_Sound(string audioEventKey)
	{
		NrCharInfoAdaptor component = base.GetComponent<NrCharInfoAdaptor>();
		if (component == null && null != base.transform.parent)
		{
			component = base.transform.parent.GetComponent<NrCharInfoAdaptor>();
		}
		bool flag = false;
		if (component != null)
		{
			if (base.gameObject.tag == "Player")
			{
				flag = true;
			}
			else if (base.gameObject.transform.parent != null && base.gameObject.transform.parent.gameObject.tag == "Player")
			{
				flag = true;
			}
		}
		TsAudioEventKeyParser tsAudioEventKeyParser = TsAudioEventKeyParser.Create(audioEventKey, component);
		if (tsAudioEventKeyParser == null)
		{
			TsLog.LogWarning("Cannot Parsing~! check Count~! goName= " + base.gameObject.name, new object[0]);
		}
		else if (tsAudioEventKeyParser.CategoryKey != "FOOTSTEP" || flag)
		{
			if (!tsAudioEventKeyParser.HasBundleKey)
			{
				TsAudioManager.Container.RequestAudioClip(tsAudioEventKeyParser.DomainKey, tsAudioEventKeyParser.CategoryKey, tsAudioEventKeyParser.AudioKey, new PostProcPerItem(this._OnComplated_DownloadAudioWWW));
			}
			else
			{
				TsAudioManager.Container.RequestAudioClip(tsAudioEventKeyParser.DomainKey, tsAudioEventKeyParser.CategoryKey, tsAudioEventKeyParser.AudioKey, tsAudioEventKeyParser.BundleKey, new PostProcPerItem(this._OnComplated_DownloadAudioWWW));
			}
		}
	}

	public void _OnComplated_DownloadAudioWWW(IDownloadedItem wItem, object obj)
	{
		if (!this)
		{
			return;
		}
		if (!base.enabled)
		{
			return;
		}
		if (wItem.canAccessAssetBundle && !wItem.isCanceled)
		{
			TsAudio.RequestData requestData = obj as TsAudio.RequestData;
			TsAudio tsAudio = TsAudioCreator.Create(requestData.baseData);
			if (tsAudio != null && this.audioSrc != null)
			{
				tsAudio._TempSet_playType = TsAudio._EPlayType.PLAY_ONE_SHOT;
				tsAudio.RefAudioSource = this.audioSrc;
				tsAudio.RefAudioClip = (wItem.mainAsset as AudioClip);
				tsAudio.PlayOneShot();
			}
			else
			{
				Debug.LogWarning("TsAudio or AudioSource is null.");
			}
		}
	}

	public void OnEvent_Trail(int iEnable)
	{
		if (this._TrailEffect == null)
		{
			this._TrailEffect = base.GetComponentInChildren<FX_MS_TrailArc>();
		}
		if (this._TrailEffect != null)
		{
			if (iEnable == 1)
			{
				this._TrailEffect.emit = true;
			}
			else
			{
				this._TrailEffect.emit = false;
				this._TrailEffect.DestoryTrail();
			}
		}
	}
}
