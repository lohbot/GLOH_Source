using System;

public class Behavior_ActorRotate : EventTriggerItem_Behavior, IEventTrigger_ActorAction, IEventTrigger_ActorName
{
	public string m_ActorName = string.Empty;

	public float m_Angle;

	public float m_ActionTime;

	public override void Init()
	{
		this.MovePosition(0f, 0f, this.m_Angle, this.m_ActionTime);
	}

	public override bool Excute()
	{
		this.m_Excute = true;
		return false;
	}

	public override bool IsPopNext()
	{
		return true;
	}

	public override string GetComment()
	{
		return string.Format("{0} 캐릭터를 {1} 각도로 {2}초 동안 회전 시킨다.", this.m_ActorName, this.m_Angle.ToString(), this.m_ActionTime.ToString());
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
		return !string.IsNullOrEmpty(this.m_ActorName);
	}

	public void SetActorName(string ActorName)
	{
		this.m_ActorName = ActorName;
	}

	public void SetPosition(float x, float y, float Angle)
	{
		this.m_Angle = Angle;
	}

	public void GetPosition(ref float x, ref float y, ref float Angle)
	{
		Angle = this.m_Angle;
	}

	public void MovePosition(float x, float y, float Angle, float time)
	{
		NrTSingleton<EventTriggerMiniDrama>.Instance.RotateActor(this.m_ActorName, Angle, time);
	}
}
