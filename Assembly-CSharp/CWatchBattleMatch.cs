using System;

public class CWatchBattleMatch : CQuestCondition
{
	public override string GetConditionText(long i64ParamVal)
	{
		string text = string.Empty;
		string result = string.Empty;
		if (base.GetParamVal() == 1L)
		{
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		}
		else if (base.GetParamVal() == 2L)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref result, new object[]
			{
				text,
				"count1",
				i64ParamVal,
				"count2",
				base.GetParamVal()
			});
		}
		return result;
	}
}
