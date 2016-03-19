using GAME;
using System;

public class NrEquipItemInfo
{
	public NkItem[] m_kItem;

	private bool m_bReceiveData;

	private int m_iUpdateCount;

	public NrEquipItemInfo()
	{
		this.m_kItem = new NkItem[6];
		this.Init();
	}

	public void Init()
	{
		for (int i = 0; i < 6; i++)
		{
			this.m_kItem[i] = new NkItem();
			this.Init(i);
		}
		this.m_bReceiveData = false;
		this.m_iUpdateCount = 0;
	}

	public void Init(int itempos)
	{
		if (itempos < 0 || itempos >= 6)
		{
			return;
		}
		this.m_kItem[itempos].Init();
		this.m_iUpdateCount++;
	}

	public void Set(NrEquipItemInfo pkInfo)
	{
		for (int i = 0; i < 6; i++)
		{
			this.m_kItem[i].SetItem(pkInfo.m_kItem[i]);
		}
		this.m_bReceiveData = pkInfo.IsReceiveData();
		this.m_iUpdateCount = pkInfo.GetUpdateCount();
	}

	public bool SetEquipItem(ITEM pItem)
	{
		if (pItem.m_nItemPos < 0 || pItem.m_nItemPos >= 6)
		{
			return false;
		}
		if (this.m_kItem[pItem.m_nItemPos].GetItemID() != pItem.m_nItemID)
		{
			this.m_kItem[pItem.m_nItemPos].SetItemInfo(pItem);
			this.m_iUpdateCount++;
			return true;
		}
		return false;
	}

	public void SetItemUnique(int itempos, int itemunique)
	{
		if (itempos < 0 || itempos >= 6)
		{
			return;
		}
		if (this.m_kItem[itempos] != null && this.m_kItem[itempos].GetItem().m_nItemUnique != itemunique)
		{
			this.m_kItem[itempos].GetItem().m_nItemID = 1000000000L;
			this.m_kItem[itempos].GetItem().m_nItemUnique = itemunique;
			this.m_kItem[itempos].GetItem().m_nItemNum = 1;
			this.m_kItem[itempos].GetItem().m_nItemPos = (int)((short)itempos);
		}
		this.m_iUpdateCount++;
	}

	public void SetItemUniqueAll(NrCharEquipPart pkEquipPart)
	{
		this.SetItemUnique(0, pkEquipPart.GetData(eAT2CharEquipPart.CHAREQUIPPART_WEAPON1));
		this.SetItemUnique(1, pkEquipPart.GetData(eAT2CharEquipPart.CHAREQUIPPART_HELMET));
		this.SetItemUnique(2, pkEquipPart.GetData(eAT2CharEquipPart.CHAREQUIPPART_ARMOR));
		this.SetItemUnique(3, pkEquipPart.GetData(eAT2CharEquipPart.CHAREQUIPPART_GLOVE));
		this.SetItemUnique(4, pkEquipPart.GetData(eAT2CharEquipPart.CHAREQUIPPART_BOOTS));
		this.SetItemUnique(5, pkEquipPart.GetData(eAT2CharEquipPart.CHAREQUIPPART_RING));
	}

	public int GetItemUnique(int itempos)
	{
		if (itempos < 0 || itempos >= 6)
		{
			return 0;
		}
		return this.m_kItem[itempos].GetItemUnique();
	}

	public void RemoveItem(int itemPos)
	{
		this.Init(itemPos);
	}

	public NkItem GetEquipItem(int itempos)
	{
		if (itempos < 0 || itempos >= 6)
		{
			return null;
		}
		return this.m_kItem[itempos];
	}

	public ITEM GetItem(int itempos)
	{
		if (itempos < 0 || itempos >= 6)
		{
			return null;
		}
		return this.m_kItem[itempos].GetItem();
	}

	public ITEM GetItemFromItemID(long ItemID)
	{
		for (int i = 0; i < 6; i++)
		{
			if (this.m_kItem[i] != null)
			{
				if (this.m_kItem[i].GetItemID() == ItemID)
				{
					return this.m_kItem[i].GetItem();
				}
			}
		}
		return null;
	}

	public void SetReceiveData(bool bReceive)
	{
		this.m_bReceiveData = bReceive;
	}

	public bool IsReceiveData()
	{
		return this.m_bReceiveData;
	}

	public int GetUpdateCount()
	{
		return this.m_iUpdateCount;
	}

	public byte RepairItemCheck()
	{
		byte result = 0;
		for (int i = 0; i < 6; i++)
		{
			if (this.m_kItem[i].GetItemUnique() != 0)
			{
				if (this.m_kItem[i].GetDurability() == 0)
				{
					return 2;
				}
				if (this.m_kItem[i].GetDurability() <= 20)
				{
					result = 1;
				}
			}
		}
		return result;
	}

	public int HaveEquipItem(ref ITEM srcItem, NkSoldierInfo solInfo)
	{
		for (int i = 0; i < 6; i++)
		{
			if (this.m_kItem[i].GetItemUnique() == 0 && NkUserInventory.GetInstance().IsHaveEquipItem((eEQUIP_ITEM)i, ref srcItem, solInfo))
			{
				return i;
			}
		}
		return -1;
	}
}
