using System;

public class CMySoldier : CQuestCondition
{
	public override bool CheckCondition(long i64Param, ref long i64ParamVal)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return false;
		}
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		if (readySolList == null)
		{
			return false;
		}
		int num = 0;
		for (int i = 0; i < 6; i++)
		{
			NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierList().GetSoldierInfo(i);
			if (soldierInfo != null)
			{
				if (soldierInfo.GetSolID() != 0L)
				{
					if (!soldierInfo.IsLeader())
					{
						num++;
					}
				}
			}
		}
		num += readySolList.GetCount();
		return (long)num >= base.GetParamVal();
	}

	public override string GetConditionText(long i64ParamVal)
	{
		string textFromQuest_Code = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		string empty = string.Empty;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return string.Empty;
		}
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		if (readySolList == null)
		{
			return string.Empty;
		}
		int num = 0;
		for (int i = 0; i < 6; i++)
		{
			NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierList().GetSoldierInfo(i);
			if (soldierInfo != null)
			{
				if (soldierInfo.GetSolID() != 0L)
				{
					if (!soldierInfo.IsLeader())
					{
						num++;
					}
				}
			}
		}
		num += readySolList.GetCount();
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromQuest_Code,
			"count1",
			num,
			"count2",
			base.GetParamVal()
		});
		return empty;
	}
}
