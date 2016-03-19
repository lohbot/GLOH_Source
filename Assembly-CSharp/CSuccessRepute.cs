using System;

public class CSuccessRepute : CQuestCondition
{
	public override bool CheckCondition(long i64Param, ref long i64ParamVal)
	{
		return false;
	}

	public override string GetConditionText(long i64ParamVal)
	{
		return string.Empty;
	}
}
