using System;

public class CExpedition : CQuestCondition
{
	public override string GetConditionText(long i64ParamVal)
	{
		string text = string.Empty;
		string empty = string.Empty;
		string empty2 = string.Empty;
		if (base.GetParamVal() == 1L)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"targetname",
				empty2,
				"count1",
				i64ParamVal,
				"count2",
				base.GetParamVal()
			});
		}
		else if (base.GetParamVal() <= 2L)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"targetname",
				empty2,
				"count1",
				i64ParamVal,
				"count2",
				base.GetParamVal()
			});
		}
		return empty;
	}
}
