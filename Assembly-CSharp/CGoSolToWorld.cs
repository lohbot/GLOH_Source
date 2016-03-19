using System;

public class CGoSolToWorld : CQuestCondition
{
	public override bool CheckCondition(long i64Param, ref long i64ParamVal)
	{
		return (i64Param == base.GetParam() && i64ParamVal >= base.GetParamVal()) || (base.GetParam() == 0L && i64ParamVal >= base.GetParamVal());
	}

	public override string GetConditionText(long i64ParamVal)
	{
		string result = string.Empty;
		if (base.GetParam() == 0L)
		{
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		}
		else if (0L < base.GetParam())
		{
		}
		return result;
	}
}
