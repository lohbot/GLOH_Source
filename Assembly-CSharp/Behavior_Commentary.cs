using System;

public class Behavior_Commentary : EventTriggerItem_Behavior
{
	public string m_Coment;

	public override void Init()
	{
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
		return this.m_Coment;
	}

	public override float ExcuteTiemSecond()
	{
		return 0f;
	}

	public override Behavior._BEHAVIORTYPE GetBehaviorType()
	{
		return Behavior._BEHAVIORTYPE.ETC;
	}
}
