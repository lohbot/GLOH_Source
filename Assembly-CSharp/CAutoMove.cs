using System;

public class CAutoMove : CQuestCondition
{
	public override bool IsServerCheck()
	{
		return false;
	}

	public override bool CheckCondition(long i64Param, ref long i64ParamVal)
	{
		return i64Param == 0L && i64ParamVal >= 1L;
	}

	public override string GetConditionText(long i64ParamVal)
	{
		return NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
	}
}
