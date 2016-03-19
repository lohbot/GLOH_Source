using System;
using UnityEngine;

public class Behavior_ActorTalk_Wait : Behavior_ActorTalk
{
	public override bool Excute()
	{
		this.m_Excute = true;
		if (this.IsPopNext())
		{
			NrTSingleton<EventTriggerMiniDrama>.Instance.HideTalk(this.m_ActorName);
			return false;
		}
		return true;
	}

	public override bool IsPopNext()
	{
		return this._StartTime != 0f && Math.Abs(this._StartTime - Time.time) >= this.m_TalkSecond;
	}
}
