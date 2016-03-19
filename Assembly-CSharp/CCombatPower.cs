using System;

public class CCombatPower : CQuestCondition
{
	public override bool CheckCondition(long i64Param, ref long i64ParamVal)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo != null)
		{
			i64ParamVal = kMyCharInfo.DepolyCombatPower;
			if (kMyCharInfo.DepolyCombatPower >= base.GetParamVal())
			{
				return true;
			}
		}
		return false;
	}

	public override string GetConditionText(long i64ParamVal)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		long num = 0L;
		if (kMyCharInfo != null)
		{
			num = kMyCharInfo.DepolyCombatPower;
		}
		string textFromQuest_Code = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromQuest_Code,
			"count1",
			num.ToString(),
			"count2",
			base.GetParamVal().ToString()
		});
		return empty;
	}
}
