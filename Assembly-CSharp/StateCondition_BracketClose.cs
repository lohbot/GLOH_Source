using System;

public class StateCondition_BracketClose : EventTriggerItem_StateConditionOperation
{
	public override string GetComment()
	{
		return ")";
	}

	public override bool Verify()
	{
		return true;
	}
}
