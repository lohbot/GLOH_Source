using System;
using UnityEngine;

public class Behavior_TimeWait : EventTriggerItem_Behavior
{
	public float m_WaitTime;

	private float _StartTime;

	public override void Init()
	{
		this._StartTime = Time.time;
	}

	public override bool Excute()
	{
		this.m_Excute = true;
		return !this.IsPopNext();
	}

	public override bool IsPopNext()
	{
		return Math.Abs(this._StartTime - Time.time) >= this.m_WaitTime;
	}

	public override string GetComment()
	{
		return string.Format("{0} 초 동안 기다린다.", this.m_WaitTime.ToString());
	}

	public override float ExcuteTiemSecond()
	{
		return 0f;
	}

	public override void Draw()
	{
	}

	public override Behavior._BEHAVIORTYPE GetBehaviorType()
	{
		return Behavior._BEHAVIORTYPE.DRAMA;
	}

	public override bool IsVaildValue()
	{
		return this.m_WaitTime > 0f;
	}
}
