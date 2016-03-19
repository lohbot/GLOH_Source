using System;

public class StateCondition_OR : EventTriggerItem_StateConditionOperation
{
	public override string GetComment()
	{
		return "OR";
	}

	public override bool Verify()
	{
		return (base.lItem != null && base.lItem.Verify()) || (base.rItem != null && base.rItem.Verify());
	}
}
