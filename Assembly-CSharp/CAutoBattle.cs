using System;

public class CAutoBattle : CQuestCondition
{
	public override string GetConditionText(long i64ParamVal)
	{
		string text = string.Empty;
		string result = string.Empty;
		if (base.GetParamVal() == 0L)
		{
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		}
		else
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
			result = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref result, new object[]
			{
				text,
				"count2",
				base.GetParamVal().ToString(),
				"count1",
				i64ParamVal.ToString()
			});
		}
		return result;
	}
}
