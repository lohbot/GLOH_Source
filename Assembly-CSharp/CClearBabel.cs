using System;

public class CClearBabel : CQuestCondition
{
	public override bool CheckCondition(long i64Param, ref long i64ParamVal)
	{
		return NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.IsBabelClear((short)base.GetParam(), (short)(base.GetParamVal() - 1L), 1);
	}

	public override string GetConditionText(long i64ParamVal)
	{
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey),
			"count1",
			base.GetParam(),
			"count2",
			base.GetParamVal()
		});
		return empty;
	}
}
