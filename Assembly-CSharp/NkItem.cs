using GAME;
using System;

public class NkItem
{
	private ITEMTYPE_INFO m_pkITEMTYPE_INFO;

	private ITEMINFO m_pkITEMINFO;

	private ITEM m_kBaseItem = new ITEM();

	public void Init()
	{
		this.m_kBaseItem.Init();
		this.m_pkITEMTYPE_INFO = null;
		this.m_pkITEMINFO = null;
	}

	public void SetItem(NkItem pkItem)
	{
		this.m_kBaseItem.Set(pkItem.GetItem());
		if (pkItem.IsValid())
		{
			this.SetItemInfo(pkItem.GetItemUnique());
		}
	}

	public void SetItemInfo(int itemunique)
	{
		this.m_pkITEMINFO = NrTSingleton<ItemManager>.Instance.GetItemInfo(itemunique);
		if (this.m_pkITEMINFO != null)
		{
			this.m_pkITEMTYPE_INFO = NrTSingleton<ItemManager>.Instance.GetItemTypeInfo(itemunique);
		}
	}

	public void SetItemInfo(ITEMINFO pItemInfo)
	{
		this.SetItemInfo(pItemInfo.m_nItemUnique);
	}

	public void SetItemInfo(ITEM pItem)
	{
		if (pItem == null)
		{
			return;
		}
		this.m_kBaseItem.Set(pItem);
		if (pItem.IsValid())
		{
			this.SetItemInfo(pItem.m_nItemUnique);
		}
	}

	public bool IsValid()
	{
		return this.m_kBaseItem.IsValid();
	}

	public void SetItemID(long ItemID)
	{
		this.m_kBaseItem.m_nItemID = ItemID;
	}

	public void SetNum(int Num)
	{
		this.m_kBaseItem.m_nItemNum = Num;
	}

	public void AddNum(int Num)
	{
		this.m_kBaseItem.m_nItemNum += Num;
	}

	public void SetEndTime(int EndTime)
	{
		this.m_kBaseItem.m_nEndTime = EndTime;
	}

	public void SetRank(int Rank)
	{
		this.m_kBaseItem.m_nRank = Rank;
	}

	public void SetDurability(int Durability)
	{
		this.m_kBaseItem.m_nDurability = ((0 >= Durability) ? 0 : Durability);
	}

	public void SetLock(int locked)
	{
		this.m_kBaseItem.m_nLock = locked;
	}

	public void SetOption(byte index, short Option)
	{
		if (index >= 0 && index < 10)
		{
			this.m_kBaseItem.m_nOption[(int)index] = (int)Option;
		}
	}

	public void ConvertItemInfo(ITEM pItem)
	{
		if (pItem == null)
		{
			return;
		}
		pItem.Set(this.m_kBaseItem);
	}

	public long GetItemID()
	{
		return this.m_kBaseItem.m_nItemID;
	}

	public int GetItemUnique()
	{
		return this.m_kBaseItem.m_nItemUnique;
	}

	public int GetItemNum()
	{
		return this.m_kBaseItem.m_nItemNum;
	}

	public int GetEndTime()
	{
		return this.m_kBaseItem.m_nEndTime;
	}

	public int GetRank()
	{
		return this.m_kBaseItem.m_nRank;
	}

	public int GetDurability()
	{
		return this.m_kBaseItem.m_nDurability;
	}

	public bool IsLock()
	{
		return this.m_kBaseItem.m_nLock != 0;
	}

	public int GetOptionID(int index)
	{
		return this.m_kBaseItem.m_nOption[index];
	}

	public ITEM GetItem()
	{
		return this.m_kBaseItem;
	}

	public ITEMTYPE_INFO GetITEMTYPE_INFO()
	{
		return this.m_pkITEMTYPE_INFO;
	}

	public ITEMINFO GetITEMINFO()
	{
		return this.m_pkITEMINFO;
	}

	public int GetItemPart()
	{
		if (this.m_pkITEMTYPE_INFO == null)
		{
			return 0;
		}
		return (int)this.m_pkITEMTYPE_INFO.ITEMPART;
	}

	public int GetItemType()
	{
		if (this.m_pkITEMTYPE_INFO == null)
		{
			return 0;
		}
		return this.m_pkITEMTYPE_INFO.ITEMTYPE;
	}

	public bool IsItemTypeATB(long itematb)
	{
		return this.m_pkITEMTYPE_INFO != null && (this.m_pkITEMTYPE_INFO.ATB & itematb) > 0L;
	}

	public int UseLevel(int nLevel)
	{
		if (this.m_pkITEMINFO.m_nUseMaxLevel > 0)
		{
			if (this.m_pkITEMINFO.m_nUseMaxLevel < nLevel)
			{
				return 807;
			}
			if (nLevel < this.m_pkITEMINFO.GetUseMinLevel(this.m_kBaseItem))
			{
				return 808;
			}
		}
		else if (nLevel < this.m_pkITEMINFO.GetUseMinLevel(this.m_kBaseItem))
		{
			return 808;
		}
		return 0;
	}

	public short GetSTR()
	{
		return this.m_pkITEMINFO.m_nSTR;
	}

	public short GetDEX()
	{
		return this.m_pkITEMINFO.m_nDEX;
	}

	public short GetINT()
	{
		return this.m_pkITEMINFO.m_nINT;
	}

	public short GetVIT()
	{
		return this.m_pkITEMINFO.m_nVIT;
	}

	public int GetAttackSpeed()
	{
		return this.m_pkITEMINFO.m_nAttackSpeed;
	}

	public int GetHitRate()
	{
		return this.GetOptionAbilityfromType(this.m_pkITEMINFO.m_nHitratePlus, 6);
	}

	public int GetEvasion()
	{
		return this.GetOptionAbilityfromType(this.m_pkITEMINFO.m_nEvasionPlus, 7);
	}

	public int GetCritical()
	{
		return this.GetOptionAbilityfromType(this.m_pkITEMINFO.m_nCriticalPlus, 3);
	}

	public int GetAddHP()
	{
		return this.GetOptionAbilityfromType(this.m_pkITEMINFO.m_nAddHP, 4);
	}

	public int GetMinDamage()
	{
		return this.GetOptionAbilityfromType(this.m_pkITEMINFO.m_nMinDamage, 1);
	}

	public int GetMaxDamage()
	{
		return this.GetOptionAbilityfromType(this.m_pkITEMINFO.m_nMaxDamage, 1);
	}

	public int GetPhysicalDefense()
	{
		return this.GetOptionAbilityfromType(this.m_pkITEMINFO.m_nDefense, 2);
	}

	public int GetMagicDefense()
	{
		return this.UpValueFromRank(this.m_pkITEMINFO.m_nMagicDefense);
	}

	public int UpValueFromRank(int value)
	{
		Item_Rank item_Rank = Item_Rank_Manager.Get_Instance().Get_RankData(this.m_pkITEMINFO.m_nQualityLevel, this.GetRank());
		if (item_Rank == null || item_Rank.ItemPerformanceRate == 0)
		{
			return value;
		}
		return item_Rank.ItemPerformanceRate * value / 100;
	}

	public int GetOptionAbilityfromType(int nValue, int optiontype)
	{
		int num = 0;
		if (this.GetITEMTYPE_INFO().OPTION1 == optiontype)
		{
			num = this.m_kBaseItem.m_nOption[0];
		}
		if (this.GetITEMTYPE_INFO().OPTION2 == optiontype)
		{
			num = this.m_kBaseItem.m_nOption[1];
		}
		if (num != 0)
		{
			nValue = nValue * num / 100;
		}
		return nValue;
	}

	public int GetMinLevel()
	{
		if (this.m_pkITEMINFO != null)
		{
			return this.m_pkITEMINFO.GetUseMinLevel(this.m_kBaseItem);
		}
		return 0;
	}

	public int GetMaxLevel()
	{
		if (this.m_pkITEMINFO != null)
		{
			return this.m_pkITEMINFO.m_nUseMaxLevel;
		}
		return 0;
	}
}
