using System;

public class Behavior_ActorMove : EventTriggerItem_Behavior, IEventTrigger_ActorAction, IEventTrigger_ActorName
{
	public string m_ActorName = string.Empty;

	public float m_DestX;

	public float m_DestY;

	public float m_Angle;

	public float m_MoveSecond;

	public override void Init()
	{
		this.MovePosition(this.m_DestX, this.m_DestY, 0f, this.m_MoveSecond);
	}

	public override bool Excute()
	{
		this.m_Excute = true;
		if (!NrTSingleton<EventTriggerMiniDrama>.Instance.IsMoveActor(this.m_ActorName))
		{
			if (this.m_Angle != 0f)
			{
				NrTSingleton<EventTriggerMiniDrama>.Instance.RotateActor(this.m_ActorName, this.m_Angle, 0.5f);
			}
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
		return string.Format("{0} 캐릭터를 X:{1}, Y:{2}로 {3}초 동안 이동 시킨다.", new object[]
		{
			this.m_ActorName,
			this.m_DestX.ToString(),
			this.m_DestY.ToString(),
			this.m_MoveSecond.ToString()
		});
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
		return !string.IsNullOrEmpty(this.m_ActorName) && (this.m_DestX != 0f || this.m_DestY != 0f);
	}

	public void SetActorName(string ActorName)
	{
		this.m_ActorName = ActorName;
	}

	public void SetPosition(float x, float y, float Angle)
	{
		this.m_DestX = x;
		this.m_DestY = y;
		this.m_Angle = Angle;
	}

	public void GetPosition(ref float x, ref float y, ref float Angle)
	{
		x = this.m_DestX;
		y = this.m_DestY;
		Angle = this.m_Angle;
	}

	public virtual void MovePosition(float x, float y, float Angle, float time)
	{
		NrTSingleton<EventTriggerMiniDrama>.Instance.MoveActor(this.m_ActorName, x, y, time);
	}
}
