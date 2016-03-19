using System;
using UnityEngine;

public class Behavior_CameraQuake : EventTriggerItem_Behavior
{
	public float m_ScaleX = 2f;

	public float m_ScaleY = 1f;

	public float m_ActionTime;

	private float _StartTime;

	public override void Init()
	{
		this._StartTime = Time.time;
		NrTSingleton<EventTriggerMiniDrama>.Instance.CameraQuake(this.m_ScaleX, this.m_ScaleY, this.m_ActionTime);
	}

	public override bool Excute()
	{
		this.m_Excute = true;
		return Math.Abs(this._StartTime - Time.time) < this.m_ActionTime;
	}

	public override bool IsPopNext()
	{
		return true;
	}

	public override string GetComment()
	{
		return string.Format("카메라를 X:{0}, Y:{1} 강도로 {2} 동안 흔든다", this.m_ScaleX.ToString(), this.m_ScaleY.ToString(), this.m_ActionTime.ToString());
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

	public override bool IsVaildValue()
	{
		return true;
	}
}
