using System;

public class CGoGenToNum : CQuestCondition
{
	public override string GetConditionText(long i64ParamVal)
	{
		string textFromQuest_Code = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return string.Empty;
		}
		int battleSoldierCount = (int)charPersonInfo.m_kSoldierList.GetBattleSoldierCount();
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromQuest_Code,
			"count1",
			battleSoldierCount,
			"count2",
			base.GetParamVal()
		});
		return empty;
	}

	public override bool CheckCondition(long i64Param, ref long i64ParamVal)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return false;
		}
		int battleSoldierCount = (int)charPersonInfo.m_kSoldierList.GetBattleSoldierCount();
		return (long)battleSoldierCount >= base.GetParamVal();
	}
}
