using System;

public class CGetLetter : CQuestCondition
{
	public override string GetConditionText(long i64ParamVal)
	{
		return NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
	}
}
