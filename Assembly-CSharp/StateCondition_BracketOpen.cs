using System;

public class StateCondition_BracketOpen : EventTriggerItem_StateConditionOperation
{
	public override string GetComment()
	{
		return "(";
	}

	public override bool Verify()
	{
		return true;
	}
}
