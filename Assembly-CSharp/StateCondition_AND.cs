using System;

public class StateCondition_AND : EventTriggerItem_StateConditionOperation
{
	public override string GetComment()
	{
		return "AND";
	}

	public override bool Verify()
	{
		return (!(base.lItem != null) || base.lItem.Verify()) && (!(base.rItem != null) || base.rItem.Verify());
	}
}
