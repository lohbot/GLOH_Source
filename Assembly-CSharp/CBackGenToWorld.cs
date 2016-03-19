using System;

public class CBackGenToWorld : CQuestCondition
{
	public override bool CheckCondition(long i64Param, ref long i64ParamVal)
	{
		return (i64Param == base.GetParam() && i64ParamVal >= base.GetParamVal()) || (base.GetParam() == 0L && i64ParamVal >= base.GetParamVal());
	}

	public override string GetConditionText(long i64ParamVal)
	{
		string text = string.Empty;
		string empty = string.Empty;
		if (0L < base.GetParam())
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		}
		else if (base.GetParam() == 0L)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"count1",
				i64ParamVal.ToString(),
				"count2",
				base.GetParamVal().ToString()
			});
		}
		return empty;
	}
}
