using System;

public class CVictoryWar : CQuestCondition
{
	public override string GetConditionText(long i64ParamVal)
	{
		string textFromQuest_Code = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromQuest_Code,
			"count1",
			i64ParamVal.ToString(),
			"count2",
			base.GetParamVal().ToString()
		});
		return empty;
	}
}
