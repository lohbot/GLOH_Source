using System;
using System.Collections.Generic;

public class CLevelSol : CQuestCondition
{
	public override bool CheckCondition(long i64Param, ref long i64ParamVal)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		int num = 0;
		if (charPersonInfo == null)
		{
			return false;
		}
		NkSoldierInfo[] kSolInfo = charPersonInfo.m_kSoldierList.m_kSolInfo;
		for (int i = 0; i < kSolInfo.Length; i++)
		{
			NkSoldierInfo nkSoldierInfo = kSolInfo[i];
			if (nkSoldierInfo.GetSolPosType() == 1 && (long)nkSoldierInfo.GetLevel() >= base.GetParam())
			{
				num++;
			}
		}
		Dictionary<long, NkSoldierInfo> list = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList().GetList();
		foreach (NkSoldierInfo current in list.Values)
		{
			if ((long)current.GetLevel() >= base.GetParam())
			{
				num++;
			}
		}
		i64Param = (long)num;
		return (long)num >= base.GetParamVal();
	}

	public override string GetConditionText(long i64ParamVal)
	{
		string textFromQuest_Code = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromQuest_Code,
			"count1",
			base.GetParam(),
			"count2",
			base.GetParamVal()
		});
		return empty;
	}
}
