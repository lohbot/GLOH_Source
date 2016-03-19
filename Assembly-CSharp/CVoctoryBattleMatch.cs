using System;

public class CVoctoryBattleMatch : CQuestCondition
{
	public override string GetConditionText(long i64ParamVal)
	{
		string text = string.Empty;
		if (base.GetParam() == 0L)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		}
		else if (base.GetParam() == 2L)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		}
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			text,
			"count1",
			i64ParamVal,
			"count2",
			base.GetParamVal()
		});
		return empty;
	}
}
