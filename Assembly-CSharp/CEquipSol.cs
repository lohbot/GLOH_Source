using GAME;
using System;

public class CEquipSol : CQuestCondition
{
	public override string GetConditionText(long i64ParamVal)
	{
		string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique((int)base.GetParam());
		string textFromQuest_Code = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		string empty = string.Empty;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		int num = 0;
		if (charPersonInfo == null)
		{
			return "No User";
		}
		for (int i = 0; i < 6; i++)
		{
			NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(i);
			for (int j = 0; j < 6; j++)
			{
				ITEM equipItem = soldierInfo.GetEquipItem(j);
				if (equipItem != null)
				{
					if ((long)equipItem.m_nItemUnique == base.GetParam())
					{
						num++;
					}
				}
			}
		}
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromQuest_Code,
			"targetname",
			itemNameByItemUnique,
			"count1",
			num,
			"count2",
			base.GetParamVal()
		});
		return empty;
	}

	public override bool CheckCondition(long i64Param, ref long i64ParamVal)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		int num = 0;
		if (charPersonInfo == null)
		{
			return false;
		}
		for (int i = 0; i < 6; i++)
		{
			NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(i);
			for (int j = 0; j < 6; j++)
			{
				ITEM equipItem = soldierInfo.GetEquipItem(j);
				if (equipItem != null)
				{
					if ((long)equipItem.m_nItemUnique == base.GetParam())
					{
						num++;
					}
				}
			}
		}
		return (long)num >= base.GetParamVal();
	}
}
