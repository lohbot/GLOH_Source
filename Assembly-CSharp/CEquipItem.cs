using GAME;
using System;

public class CEquipItem : CQuestCondition
{
	public override bool CheckCondition(long i64Param, ref long i64ParamVal)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return false;
		}
		int num = 0;
		if (base.GetParam() == 0L)
		{
			for (int i = 0; i < 6; i++)
			{
				for (int j = 0; j < 6; j++)
				{
					NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(i);
					if (soldierInfo != null && soldierInfo.GetSolID() != 0L)
					{
						int nItemUnique = soldierInfo.GetEquipItem(j).m_nItemUnique;
						if (nItemUnique > 0)
						{
							if (NrTSingleton<ItemManager>.Instance.GetItemPartByItemUnique(nItemUnique) == eITEM_PART.ITEMPART_WEAPON)
							{
								num++;
							}
							if (base.GetParamVal() <= (long)num)
							{
								return true;
							}
						}
					}
				}
			}
		}
		else
		{
			for (int k = 0; k < 6; k++)
			{
				for (int l = 0; l < 6; l++)
				{
					if ((long)charPersonInfo.GetSoldierInfo(k).GetEquipItem(l).m_nItemUnique == base.GetParam())
					{
						num++;
					}
					if (base.GetParamVal() <= (long)num)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public override string GetConditionText(long i64ParamVal)
	{
		string text = string.Empty;
		string empty = string.Empty;
		if (0L < base.GetParam())
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
			string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique((int)base.GetParam());
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"targetname",
				itemNameByItemUnique
			});
		}
		else if (base.GetParam() == 0L)
		{
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			if (charPersonInfo == null)
			{
				return " ";
			}
			int num = 0;
			for (int i = 0; i < 6; i++)
			{
				for (int j = 0; j < 6; j++)
				{
					NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(i);
					if (soldierInfo != null && soldierInfo.GetSolID() != 0L)
					{
						int nItemUnique = soldierInfo.GetEquipItem(j).m_nItemUnique;
						if (nItemUnique > 0)
						{
							if (NrTSingleton<ItemManager>.Instance.GetItemPartByItemUnique(nItemUnique) == eITEM_PART.ITEMPART_WEAPON)
							{
								num++;
							}
							if (base.GetParamVal() <= (long)num)
							{
								num = (int)base.GetParamVal();
								break;
							}
						}
					}
				}
			}
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"count1",
				num,
				"count2",
				base.GetParamVal()
			});
		}
		return empty;
	}
}
