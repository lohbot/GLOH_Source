using System;

public class CPassTurn : CQuestCondition
{
	public override string GetConditionText(long i64ParamVal)
	{
		string text = string.Empty;
		string empty = string.Empty;
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
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
