using System;

public class CCareAnimal : CQuestCondition
{
	public override string GetConditionText(long i64ParamVal)
	{
		if (NrTSingleton<NkQuestManager>.Instance == null)
		{
			return string.Empty;
		}
		string empty = string.Empty;
		string textFromQuest_Code = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		if (base.GetParam() == 0L)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromQuest_Code,
				"count1",
				i64ParamVal.ToString(),
				"count2",
				base.GetParamVal().ToString()
			});
		}
		return empty;
	}
}
