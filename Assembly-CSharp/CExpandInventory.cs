using System;

public class CExpandInventory : CQuestCondition
{
	public override bool CheckCondition(long i64Param, ref long i64ParamVal)
	{
		return true;
	}

	public override string GetConditionText(long i64ParamVal)
	{
		return string.Empty;
	}
}
