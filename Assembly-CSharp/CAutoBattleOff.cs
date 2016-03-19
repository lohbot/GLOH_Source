using System;

public class CAutoBattleOff : CQuestCondition
{
	public override string GetConditionText(long i64ParamVal)
	{
		string empty = string.Empty;
		return NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
	}
}
