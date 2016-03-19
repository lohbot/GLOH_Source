using System;

public class CMakeSkillCon : CQuestCondition
{
	public override string GetConditionText(long i64ParamVal)
	{
		string empty = string.Empty;
		string text = string.Empty;
		if (base.GetParam() == 1L)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"count1",
				base.GetParamVal()
			});
		}
		else if (base.GetParam() == 2L)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"count1",
				base.GetParamVal()
			});
		}
		return empty;
	}

	public override bool CheckCondition(long i64Param, ref long i64ParamVal)
	{
		return false;
	}
}
