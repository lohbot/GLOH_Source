using System;
using UnityEngine;

public class Behavior_CameraFade : EventTriggerItem_Behavior
{
	public float m_FadeInTime;

	public float m_DurationTime;

	public float m_FadeOutTime;

	public float m_Red;

	public float m_Green;

	public float m_Blue;

	private float _StartTime;

	public override void Init()
	{
		NrTSingleton<EventTriggerMiniDrama>.Instance.CameraFade(this.m_Red, this.m_Green, this.m_Blue, this.m_FadeInTime, this.m_DurationTime, this.m_FadeOutTime);
		this._StartTime = Time.time;
	}

	public override bool Excute()
	{
		this.m_Excute = true;
		return Math.Abs(this._StartTime - Time.time) >= this.m_FadeInTime + this.m_DurationTime + this.m_FadeOutTime && false;
	}

	public override bool IsPopNext()
	{
		return true;
	}

	public override string GetComment()
	{
		return string.Format("화면을 페이드 한다.", new object[0]);
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
		return Behavior._BEHAVIORTYPE.CAMERA;
	}
}
