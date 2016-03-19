using System;

public class Behavior_ActorHide : EventTriggerItem_Behavior, IEventTrigger_ActorName
{
	public string m_ActorName = string.Empty;

	public override void Init()
	{
		NrTSingleton<EventTriggerMiniDrama>.Instance.ShowActor(this.m_ActorName, false);
	}

	public override bool Excute()
	{
		this.m_Excute = true;
		return !this.IsPopNext();
	}

	public override bool IsPopNext()
	{
		return true;
	}

	public override string GetComment()
	{
		return string.Format("{0} 캐릭터를 숨긴다.", this.m_ActorName);
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
}
