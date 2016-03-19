using GAME;
using System;
using UnityEngine;

public class Behavior_ActorAni_Wait : Behavior_ActorAni
{
	public override void Init()
	{
		base.Init();
	}

	public override bool Excute()
	{
		this.m_Excute = true;
		if (this.IsPopNext())
		{
			NrTSingleton<EventTriggerMiniDrama>.Instance.AniActor(this.m_ActorName, eCharAnimationType.Stay1.ToString());
			return false;
		}
		return true;
	}

	public override bool IsPopNext()
	{
		return this._StartTime != 0f && Math.Abs(this._StartTime - Time.time) >= base._Time;
	}
}
