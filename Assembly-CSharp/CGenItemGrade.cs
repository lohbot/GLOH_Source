using System;

public class CGenItemGrade : CQuestCondition
{
	public override string GetConditionText(long i64ParamVal)
	{
		string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique((int)base.GetParam());
		string textFromQuest_Code = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		string empty = string.Empty;
		int num = 0;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return "No User";
		}
		for (int i = 0; i < 6; i++)
		{
			NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(i);
			if (soldierInfo.GetSolPosType() == 1)
			{
				bool flag = false;
				for (int j = 0; j < 6; j++)
				{
					if (soldierInfo.GetEquipItemInfo() != null && (long)soldierInfo.GetEquipItem(j).m_nItemUnique == base.GetParam())
					{
						num = soldierInfo.GetEquipItem(j).m_nRank;
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
		}
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromQuest_Code,
			"targetname",
			itemNameByItemUnique,
			"count1",
			base.GetParamVal(),
			"count2",
			num
		});
		return empty;
	}

	public override bool CheckCondition(long i64Param, ref long i64ParamVal)
	{
		return NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1) != null;
	}
}
