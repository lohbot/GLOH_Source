using System;

public class Behavior_ActorMake : EventTriggerItem_Behavior, IEventTrigger_ActorAction, IEventTrigger_ActorMake
{
	public string m_ActorName = string.Empty;

	public string m_CharKind = string.Empty;

	public float m_X;

	public float m_Y;

	public int m_Angle;

	public bool m_Hide;

	public override void Init()
	{
		this.MakeActor(this.m_ActorName, this.m_X, this.m_Y, (float)this.m_Angle, this.m_Hide);
	}

	public override bool Excute()
	{
		this.m_Excute = true;
		return !NrTSingleton<EventTriggerMiniDrama>.Instance.IsMakeActor(this.m_ActorName);
	}

	public override bool IsPopNext()
	{
		return true;
	}

	public override string GetComment()
	{
		if (!this.m_Hide)
		{
			return string.Format("이름이 {0}인 {1} 캐릭터를 X:{2}, Y:{3}에 {4}를 봐라보도록 생성한다.", new object[]
			{
				this.m_ActorName,
				this.m_CharKind,
				this.m_X.ToString(),
				this.m_Y.ToString(),
				this.m_Angle.ToString()
			});
		}
		return string.Format("이름이 {0}인 {1} 캐릭터를 X:{2}, Y:{3}에 {4}를 봐라보도록 생성후 숨긴다.", new object[]
		{
			this.m_ActorName,
			this.m_CharKind,
			this.m_X.ToString(),
			this.m_Y.ToString(),
			this.m_Angle.ToString()
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
		return !string.IsNullOrEmpty(this.m_CharKind) && !string.IsNullOrEmpty(this.m_ActorName);
	}

	public void SetMakeActorCode(string ActorCode)
	{
		this.m_ActorName = ActorCode;
		this.m_CharKind = ActorCode;
	}

	public string[] GetMakeActorName()
	{
		return new string[]
		{
			this.m_ActorName
		};
	}

	public void SetActorName(string ActorName)
	{
		this.m_ActorName = ActorName;
	}

	public void MakeActor(string ActorName, float x, float y, float Angle, bool Hide)
	{
		NrTSingleton<EventTriggerMiniDrama>.Instance.MakeActor(this.m_ActorName, this.m_CharKind, x, y, (short)Angle, Hide);
	}

	public void SetPosition(float x, float y, float Angle)
	{
		this.m_X = x;
		this.m_Y = y;
		this.m_Angle = (int)((short)Angle);
	}

	public void GetPosition(ref float x, ref float y, ref float Angle)
	{
		x = this.m_X;
		y = this.m_Y;
		Angle = (float)this.m_Angle;
	}

	public void MovePosition(float x, float y, float Angle, float time)
	{
	}
}
