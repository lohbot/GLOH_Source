using System;
using UnityEngine;

public class Behavior_ActorCaption_Wait : Behavior_ActorCaption
{
	public override bool Excute()
	{
		this.m_Excute = true;
		if (this.IsPopNext())
		{
			NrTSingleton<EventTriggerMiniDrama>.Instance.HideCaption(this.m_TalkKey);
			return false;
		}
		return true;
	}

	public override bool IsPopNext()
	{
		return this._StartTime != 0f && Math.Abs(this._StartTime - Time.time) >= this.m_TalkSecond;
	}
}
