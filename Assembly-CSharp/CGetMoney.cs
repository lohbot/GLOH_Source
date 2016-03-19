using System;

public class CGetMoney : CQuestCondition
{
	public override bool CheckCondition(long i64Param, ref long i64ParamVal)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		long money = kMyCharInfo.m_Money;
		i64ParamVal = money;
		return money >= base.GetParamVal();
	}

	public override string GetConditionText(long i64ParamVal)
	{
		string text = string.Empty;
		string empty = string.Empty;
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			text,
			"count1",
			base.GetParamVal().ToString(),
			"count2",
			kMyCharInfo.m_Money.ToString(),
			"count1",
			base.GetParamVal().ToString()
		});
		return empty;
	}
}
