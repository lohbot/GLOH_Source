using System;

public class CGoSolToWorld2 : CQuestCondition
{
	public override bool CheckCondition(long i64Param, ref long i64ParamVal)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return false;
		}
		for (int i = 0; i < 6; i++)
		{
			NkSoldierInfo nkSoldierInfo = charPersonInfo.m_kSoldierList.m_kSolInfo[i];
			if (nkSoldierInfo.IsValid())
			{
				if (nkSoldierInfo.GetSolPosType() == 1 && (long)nkSoldierInfo.GetCharKind() == base.GetParam())
				{
					return true;
				}
			}
		}
		return false;
	}

	public override string GetConditionText(long i64ParamVal)
	{
		string textFromQuest_Code = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		string empty = string.Empty;
		string name = NrTSingleton<NrCharKindInfoManager>.Instance.GetName((int)base.GetParam());
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromQuest_Code,
			"targetname",
			name
		});
		return empty;
	}
}
