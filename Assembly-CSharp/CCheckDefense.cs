using GAME;
using System;

public class CCheckDefense : CQuestCondition
{
	public override string GetConditionText(long i64ParamVal)
	{
		string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique((int)base.GetParam());
		string textFromQuest_Code = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		string empty = string.Empty;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return "No User";
		}
		int num = 0;
		for (int i = 0; i < 6; i++)
		{
			NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(i);
			if (soldierInfo.GetSolPosType() == 1)
			{
				for (int j = 0; j < 6; j++)
				{
					if (soldierInfo.GetEquipItemInfo() != null && (long)soldierInfo.GetEquipItem(j).m_nItemUnique == base.GetParam())
					{
						int num2 = Protocol_Item.Get_Defense(soldierInfo.GetEquipItem(j));
						if (num > num2)
						{
							num = num2;
						}
					}
				}
			}
		}
		NkUserInventory instance = NkUserInventory.GetInstance();
		if (instance == null)
		{
			return string.Empty;
		}
		for (short num3 = 0; num3 < 30; num3 += 1)
		{
			ITEM item = instance.GetItem(1, (int)num3);
			if (item != null)
			{
				int num4 = Protocol_Item.Get_Defense(item);
				if (num > num4)
				{
					num = num4;
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
		if (charPersonInfo == null)
		{
			return false;
		}
		int num = 0;
		for (int i = 0; i < 6; i++)
		{
			NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(i);
			if (soldierInfo.GetSolPosType() == 1)
			{
				for (int j = 0; j < 6; j++)
				{
					if (soldierInfo.GetEquipItemInfo() != null && (long)soldierInfo.GetEquipItem(j).m_nItemUnique == base.GetParam())
					{
						int num2 = Protocol_Item.Get_Defense(soldierInfo.GetEquipItem(j));
						if (num > num2)
						{
							num = num2;
						}
					}
				}
			}
		}
		NkUserInventory instance = NkUserInventory.GetInstance();
		if (instance == null)
		{
			return false;
		}
		for (short num3 = 0; num3 < 30; num3 += 1)
		{
			ITEM item = instance.GetItem(1, (int)num3);
			if (item != null)
			{
				int num4 = Protocol_Item.Get_Defense(item);
				if (num > num4)
				{
					num = num4;
				}
			}
		}
		return (long)num >= base.GetParamVal();
	}
}
