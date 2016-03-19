using System;

public class CFollowChar : CQuestCondition
{
	public override bool CheckCondition(long i64Param, ref long i64PramVal)
	{
		if (i64Param == base.GetParam())
		{
			ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo((int)i64Param);
			if (itemInfo != null && itemInfo.m_strQuestItemFunc == "F_followchar")
			{
				NrCharUser nrCharUser = (NrCharUser)NrTSingleton<NkCharManager>.Instance.GetChar(1);
				if (nrCharUser != null && nrCharUser.GetSubChsrKind(0) == itemInfo.m_nQuestFuncParam)
				{
					i64PramVal = (long)itemInfo.m_nQuestFuncParam;
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
		string text = "none";
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo((int)base.GetParam());
		if (itemInfo != null && itemInfo.m_strQuestItemFunc == "F_followchar")
		{
			NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(itemInfo.m_nQuestFuncParam);
			text = charKindInfo.GetName();
		}
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromQuest_Code,
			"targetname",
			text
		});
		return empty;
	}
}
