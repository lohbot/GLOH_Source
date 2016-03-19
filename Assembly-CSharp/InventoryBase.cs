using GAME;
using System;

public class InventoryBase
{
	public ITEM[] m_kInvenItem;

	public InventoryBase()
	{
		this.m_kInvenItem = new ITEM[ItemDefine.INVENTORY_ITEMSLOT_MAX];
		this.Init();
	}

	public void Init()
	{
		for (int i = 0; i < ItemDefine.INVENTORY_ITEMSLOT_MAX; i++)
		{
			this.m_kInvenItem[i] = new ITEM();
			this.Init(i);
		}
	}

	public void Init(int itempos)
	{
		if (itempos < 0 || itempos >= ItemDefine.INVENTORY_ITEMSLOT_MAX)
		{
			return;
		}
		this.m_kInvenItem[itempos].Init();
	}

	public void SetItem(ITEM a_cItem)
	{
		if (a_cItem == null)
		{
			return;
		}
		if (a_cItem.m_nItemPos < 0 || a_cItem.m_nItemPos >= ItemDefine.INVENTORY_ITEMSLOT_MAX)
		{
			return;
		}
		this.m_kInvenItem[a_cItem.m_nItemPos] = a_cItem;
	}

	public void RemoveItemByPos(int ItemPos)
	{
		if (ItemPos < 0 || ItemPos >= ItemDefine.INVENTORY_ITEMSLOT_MAX)
		{
			return;
		}
		this.m_kInvenItem[ItemPos].Init();
	}

	public ITEM GetItemByUnique(int ItemUnique)
	{
		for (int i = 0; i < ItemDefine.INVENTORY_ITEMSLOT_MAX; i++)
		{
			if (this.m_kInvenItem[i].m_nItemUnique == ItemUnique)
			{
				return this.m_kInvenItem[i];
			}
		}
		return null;
	}

	public ITEM GetItem(int ItemPos)
	{
		if (ItemPos < 0 || ItemPos >= ItemDefine.INVENTORY_ITEMSLOT_MAX)
		{
			return null;
		}
		if (this.m_kInvenItem[ItemPos].m_nItemUnique > 0)
		{
			return this.m_kInvenItem[ItemPos];
		}
		return null;
	}

	public int GetItemCount()
	{
		int num = 0;
		for (int i = 0; i < ItemDefine.INVENTORY_ITEMSLOT_MAX; i++)
		{
			if (this.m_kInvenItem[i].m_nItemUnique > 0)
			{
				num++;
			}
		}
		return num;
	}

	public int GetItemPosFromItemunique(int itemunique)
	{
		for (int i = 0; i < ItemDefine.INVENTORY_ITEMSLOT_MAX; i++)
		{
			if (this.m_kInvenItem[i].m_nItemUnique == itemunique)
			{
				return this.m_kInvenItem[i].m_nItemPos;
			}
		}
		return -1;
	}

	public ITEM GetItemFromItemunique(int itemunique)
	{
		for (int i = 0; i < ItemDefine.INVENTORY_ITEMSLOT_MAX; i++)
		{
			if (this.m_kInvenItem[i].m_nItemUnique == itemunique)
			{
				return this.m_kInvenItem[i];
			}
		}
		return null;
	}

	public ITEM GetItemByATB(long itematb)
	{
		for (int i = 0; i < ItemDefine.INVENTORY_ITEMSLOT_MAX; i++)
		{
			ITEM iTEM = this.m_kInvenItem[i];
			if (iTEM != null)
			{
				if (iTEM.IsValid())
				{
					if (NrTSingleton<ItemManager>.Instance.IsItemATB(iTEM.m_nItemUnique, itematb))
					{
						return iTEM;
					}
				}
			}
		}
		return null;
	}

	public ITEM GetItemFromItemID(long ItemID)
	{
		for (int i = 0; i < ItemDefine.INVENTORY_ITEMSLOT_MAX; i++)
		{
			ITEM iTEM = this.m_kInvenItem[i];
			if (iTEM != null)
			{
				if (iTEM.IsValid())
				{
					if (iTEM.m_nItemID == ItemID)
					{
						return iTEM;
					}
				}
			}
		}
		return null;
	}

	public int GetItemCount(int itemunique)
	{
		int num = 0;
		for (int i = 0; i < ItemDefine.INVENTORY_ITEMSLOT_MAX; i++)
		{
			if (this.m_kInvenItem[i].m_nItemUnique == itemunique)
			{
				num += this.m_kInvenItem[i].m_nItemNum;
			}
		}
		return num;
	}

	public int GetItemCountFromATB(long itematb)
	{
		int num = 0;
		for (int i = 0; i < ItemDefine.INVENTORY_ITEMSLOT_MAX; i++)
		{
			ITEM iTEM = this.m_kInvenItem[i];
			if (iTEM != null)
			{
				if (iTEM.IsValid())
				{
					if (NrTSingleton<ItemManager>.Instance.IsItemATB(iTEM.m_nItemUnique, itematb))
					{
						num += iTEM.m_nItemNum;
					}
				}
			}
		}
		return num;
	}

	public int Get_First_ItemCnt(int itemunique)
	{
		for (int i = 0; i < ItemDefine.INVENTORY_ITEMSLOT_MAX; i++)
		{
			if (this.m_kInvenItem[i].m_nItemUnique == itemunique)
			{
				return this.m_kInvenItem[i].m_nItemNum;
			}
		}
		return 0;
	}

	public ITEM GetFristItemByUniqueLowRank(int itemunique)
	{
		ITEM iTEM = null;
		for (int i = 0; i < ItemDefine.INVENTORY_ITEMSLOT_MAX; i++)
		{
			if (this.m_kInvenItem[i].m_nItemUnique == itemunique)
			{
				if (iTEM == null)
				{
					iTEM = this.m_kInvenItem[i];
				}
				else if (iTEM != null && iTEM.m_nRank > this.m_kInvenItem[i].m_nRank)
				{
					iTEM = this.m_kInvenItem[i];
				}
			}
		}
		return iTEM;
	}

	public ITEM GetBatterItemByUnique(ITEM item, short level)
	{
		ITEMINFO iTEMINFO = NrTSingleton<ItemManager>.Instance.GetItemInfo(item.m_nItemUnique);
		if (iTEMINFO == null)
		{
			return null;
		}
		ITEM iTEM = null;
		for (int i = 0; i < ItemDefine.INVENTORY_ITEMSLOT_MAX; i++)
		{
			if (NrTSingleton<ItemManager>.Instance.GetItemMinLevelFromItem(this.m_kInvenItem[i]) <= (int)level)
			{
				ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(this.m_kInvenItem[i].m_nItemUnique);
				if (itemInfo != null)
				{
					if (itemInfo.m_nItemType == iTEMINFO.m_nItemType)
					{
						if (iTEMINFO.m_nMinDamage != 0 && itemInfo.m_nMinDamage != 0)
						{
							int num;
							if (iTEM != null)
							{
								num = Tooltip_Dlg.GetOptionValue(iTEM.m_nOption[0], iTEMINFO.m_nMinDamage);
								num += Tooltip_Dlg.GetOptionValue(iTEM.m_nOption[0], iTEMINFO.m_nMaxDamage);
							}
							else
							{
								num = Tooltip_Dlg.GetOptionValue(item.m_nOption[0], iTEMINFO.m_nMinDamage);
								num += Tooltip_Dlg.GetOptionValue(item.m_nOption[0], iTEMINFO.m_nMaxDamage);
							}
							num /= 2;
							int num2 = Tooltip_Dlg.GetOptionValue(this.m_kInvenItem[i].m_nOption[0], itemInfo.m_nMinDamage);
							num2 += Tooltip_Dlg.GetOptionValue(this.m_kInvenItem[i].m_nOption[0], itemInfo.m_nMaxDamage);
							num2 /= 2;
							if (num < num2)
							{
								iTEM = this.m_kInvenItem[i];
								iTEMINFO = itemInfo;
							}
						}
						else if (iTEMINFO.m_nDefense != 0 && itemInfo.m_nDefense != 0)
						{
							int optionValue;
							if (iTEM != null)
							{
								optionValue = Tooltip_Dlg.GetOptionValue(iTEM.m_nOption[0], iTEMINFO.m_nDefense);
							}
							else
							{
								optionValue = Tooltip_Dlg.GetOptionValue(item.m_nOption[0], iTEMINFO.m_nDefense);
							}
							int optionValue2 = Tooltip_Dlg.GetOptionValue(this.m_kInvenItem[i].m_nOption[0], itemInfo.m_nDefense);
							if (optionValue < optionValue2)
							{
								iTEM = this.m_kInvenItem[i];
								iTEMINFO = itemInfo;
							}
						}
					}
				}
			}
		}
		return iTEM;
	}

	public void SetItemNum(int ItemPos, int a_shItemNum)
	{
		if (ItemPos < 0 || ItemPos >= ItemDefine.INVENTORY_ITEMSLOT_MAX)
		{
			return;
		}
		this.m_kInvenItem[ItemPos].m_nItemNum = a_shItemNum;
	}

	public int Get_Item_Durability(int itemunique)
	{
		for (int i = 0; i < ItemDefine.INVENTORY_ITEMSLOT_MAX; i++)
		{
			if (this.m_kInvenItem[i].m_nItemUnique == itemunique)
			{
				return this.m_kInvenItem[i].m_nDurability;
			}
		}
		return -1;
	}

	public int Get_Item_Rank(int itemunique)
	{
		for (int i = 0; i < ItemDefine.INVENTORY_ITEMSLOT_MAX; i++)
		{
			if (this.m_kInvenItem[i].m_nItemUnique == itemunique)
			{
				return this.m_kInvenItem[i].m_nRank;
			}
		}
		return 0;
	}
}
