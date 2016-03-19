using GAME;
using System;
using UnityEngine;

public class Behavior_ActorAni : EventTriggerItem_Behavior, IEventTrigger_ActorName
{
	public string m_ActorName = string.Empty;

	public string m_ActorAni = string.Empty;

	public float m_AniTime;

	protected float _AniPlayTime;

	protected float _StartTime;

	protected float _Time
	{
		get
		{
			return (this.m_AniTime <= 0f) ? this._AniPlayTime : this.m_AniTime;
		}
	}

	public override void Init()
	{
		this._AniPlayTime = NrTSingleton<EventTriggerMiniDrama>.Instance.AniActor(this.m_ActorName, this.m_ActorAni);
		this._StartTime = Time.time;
	}

	public override bool Excute()
	{
		this.m_Excute = true;
		if (Math.Abs(this._StartTime - Time.time) >= this._Time)
		{
			NrTSingleton<EventTriggerMiniDrama>.Instance.AniActor(this.m_ActorName, eCharAnimationType.Stay1.ToString());
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
		return string.Format("{0} 캐릭터가 {1} 동작을 {2} 동안 한다.", this.m_ActorName, this.m_ActorAni, this._Time);
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
		return true;
	}

	public void SetActorName(string ActorName)
	{
		this.m_ActorName = ActorName;
	}
}
