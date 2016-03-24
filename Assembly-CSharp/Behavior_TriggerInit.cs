using System;

public class Behavior_TriggerInit : EventTriggerItem_Behavior
{
	public override void Init()
	{
	}

	public override bool IsVaildValue()
	{
		return true;
	}

	public override bool Excute()
	{
		EventTrigger componentInParent = base.gameObject.GetComponentInParent<EventTrigger>();
		if (componentInParent == null)
		{
			return false;
		}
		componentInParent.TriggerOn = false;
		return true;
	}

	public override bool IsPopNext()
	{
		return true;
	}

	public override string GetComment()
	{
		return "트리거 초기화";
	}

	public override void Draw()
	{
	}

	public override Behavior._BEHAVIORTYPE GetBehaviorType()
	{
		return Behavior._BEHAVIORTYPE.ETC;
	}

	public override float ExcuteTiemSecond()
	{
		return 0f;
	}
}
