using System;
using TsBundle;
using UnityEngine;

public class Behavior_ActorTalk : EventTriggerItem_Behavior, IEventTrigger_ActorName, IEventTrigger_ActorText
{
	public string m_ActorName = string.Empty;

	public string m_TalkKey = string.Empty;

	public float m_TalkSecond = 3f;

	protected float _StartTime;

	public override void Init()
	{
		if (TsAudioManager.Container.TryToMakeAudioBaseData("MINI", "VOICE", this.m_TalkKey) != null)
		{
			TsAudioManager.Container.RequestAudioClip("MINI", "VOICE", this.m_TalkKey, new PostProcPerItem(this.OnEventAudioClip));
		}
		else
		{
			this.OnEventAudioClip(null, null);
		}
	}

	public void OnEventAudioClip(IDownloadedItem wItem, object obj)
	{
		if (wItem != null && !wItem.isCanceled)
		{
			TsAudio.RequestData requestData = obj as TsAudio.RequestData;
			TsAudio tsAudio = TsAudioCreator.Create(requestData.baseData);
			if (tsAudio != null)
			{
				if (wItem.mainAsset == null)
				{
					TsLog.LogWarning("wItem.mainAsset is null -> Path = {0}", new object[]
					{
						wItem.assetPath
					});
				}
				else
				{
					tsAudio.RefAudioClip = (wItem.mainAsset as AudioClip);
					wItem.unloadImmediate = true;
					this.m_TalkSecond = tsAudio.RefAudioClip.length;
				}
				tsAudio.PlayClipAtPoint(Vector3.zero);
			}
			else
			{
				TsAudioManager.Container.RequestAudioClip("TRIGGER", "COMMON", "Trigger_SpeechBubble", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
			}
		}
		this._StartTime = Time.time;
		NrTSingleton<EventTriggerMiniDrama>.Instance.ShowTalk(this.m_ActorName, this.m_TalkKey, this.m_TalkSecond);
	}

	public override bool Excute()
	{
		this.m_Excute = true;
		if (this._StartTime != 0f && Math.Abs(this._StartTime - Time.time) >= this.m_TalkSecond)
		{
			NrTSingleton<EventTriggerMiniDrama>.Instance.HideTalk(this.m_ActorName);
			return false;
		}
		return true;
	}

	public override bool IsPopNext()
	{
		return true;
	}

	public override string GetComment()
	{
		return string.Format("{0} 캐릭터가 {1} 대사를 {2}초 동안 출력한다.", this.m_ActorName, this.m_TalkKey, this.m_TalkSecond.ToString());
	}

	public override float ExcuteTiemSecond()
	{
		return 0f;
	}

	public override Behavior._BEHAVIORTYPE GetBehaviorType()
	{
		return Behavior._BEHAVIORTYPE.OBJECT;
	}

	public override bool IsVaildValue()
	{
		return !string.IsNullOrEmpty(this.m_ActorName) && !string.IsNullOrEmpty(this.m_TalkKey) && this.m_TalkSecond > 0f;
	}

	public void SetActorName(string ActorName)
	{
		this.m_ActorName = ActorName;
	}

	public void SetTextKey(string TextKey)
	{
		this.m_TalkKey = TextKey;
	}
}
