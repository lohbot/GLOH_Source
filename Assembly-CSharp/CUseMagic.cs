using System;

public class CUseMagic : CQuestCondition
{
	public override string GetConditionText(long i64ParamVal)
	{
		string text = string.Empty;
		string empty = string.Empty;
		if (base.GetParam() == 0L)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"count1",
				i64ParamVal,
				"count2",
				base.GetParamVal()
			});
		}
		else if (base.GetParam() > 0L)
		{
		}
		return empty;
	}
}
