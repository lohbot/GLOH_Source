using System;
using TsBundle;
using UnityEngine;

namespace WellFired
{
	[USequencerEvent("LOH/LOH Caption"), USequencerEventHideDuration, USequencerFriendlyName("LOH Caption")]
	public class NUSCaption : USEventBase
	{
		public string m_ActorName = string.Empty;

		public string m_TalkKey = string.Empty;

		public float m_TalkSecond = 3f;

		protected float _StartTime;

		public override void FireEvent()
		{
			base.Duration = this.m_TalkSecond + 1f;
			this._StartTime = 0f;
			Behavior_ActorMake behavior_ActorMake = new Behavior_ActorMake();
			behavior_ActorMake.SetMakeActorCode(this.m_ActorName);
			behavior_ActorMake.Init();
			if (TsAudioManager.Container.TryToMakeAudioBaseData("MINI", "VOICE", this.m_TalkKey) != null)
			{
				TsAudioManager.Container.RequestAudioClip("MINI", "VOICE", this.m_TalkKey, new PostProcPerItem(this.OnEventAudioClip));
			}
			else
			{
				this.OnEventAudioClip(null, null);
			}
		}

		public override void ProcessEvent(float deltaTime)
		{
			if (this._StartTime != 0f && Math.Abs(this._StartTime - Time.time) >= this.m_TalkSecond)
			{
				NrTSingleton<EventTriggerMiniDrama>.Instance.HideCaption(this.m_TalkKey);
				base.Duration = 0f;
				return;
			}
		}

		public void OnEventAudioClip(IDownloadedItem wItem, object obj)
		{
			if (wItem != null && !wItem.isCanceled)
			{
				TsAudio.RequestData requestData = obj as TsAudio.RequestData;
				requestData.baseData.Tag = TsAudioManager.MINI_SOUND;
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
			NrTSingleton<EventTriggerMiniDrama>.Instance.ShowCaption(this.m_ActorName, this.m_TalkKey, this.m_TalkSecond);
		}

		public override void StopEvent()
		{
			NrTSingleton<EventTriggerMiniDrama>.Instance.HideCaption(this.m_TalkKey);
			base.Duration = 0f;
		}
	}
}
