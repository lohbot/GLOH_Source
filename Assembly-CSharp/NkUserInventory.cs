using GAME;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityForms;

public class NkUserInventory
{
	private Action m_deUpdate;

	public static NkUserInventory instance;

	private bool m_InitComplete;

	public int m_UpdateCount;

	private int[] m_InvenSlot = new int[30];

	private InventoryBase[] m_Inventory;

	public NkUserInventory()
	{
		this.m_Inventory = new InventoryBase[7];
		for (int i = 0; i < 7; i++)
		{
			this.m_Inventory[i] = new InventoryBase();
		}
	}

	public int GetInventoryIndex(int ItemPosType)
	{
		int result = -1;
		switch (ItemPosType)
		{
		case 1:
			result = 0;
			break;
		case 2:
			result = 1;
			break;
		case 3:
			result = 2;
			break;
		case 4:
			result = 3;
			break;
		case 5:
			result = 4;
			break;
		case 6:
			result = 5;
			break;
		case 7:
			result = 6;
			break;
		}
		return result;
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	public static NkUserInventory GetInstance()
	{
		if (NkUserInventory.instance == null)
		{
			NkUserInventory.instance = new NkUserInventory();
		}
		return NkUserInventory.instance;
	}

	public void Inventory_Refresh(int a_byPosType, int a_shPosItem)
	{
		Inventory_Dlg inventory_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.INVENTORY_DLG) as Inventory_Dlg;
		if (inventory_Dlg != null)
		{
			inventory_Dlg.Refresh_PosType(a_byPosType, a_shPosItem);
		}
		this.m_UpdateCount++;
		if (this.m_deUpdate != null)
		{
			this.m_deUpdate();
		}
	}

	public void Add_Update_Delegate(Action a_deUpate)
	{
		this.m_deUpdate = (Action)Delegate.Combine(this.m_deUpdate, a_deUpate);
	}

	public void Remove_Update_Delegate(Action a_deUpate)
	{
		this.m_deUpdate = (Action)Delegate.Remove(this.m_deUpdate, a_deUpate);
	}

	public bool GetInitComplete()
	{
		return this.m_InitComplete;
	}

	public void SetInitComplete()
	{
		this.m_InitComplete = true;
	}

	public bool CanBlinkTab(int SetType)
	{
		bool result = false;
		switch (SetType)
		{
		case 3:
		case 4:
		case 7:
		case 8:
		case 10:
		case 11:
		case 12:
		case 13:
			result = true;
			break;
		}
		return result;
	}

	public void SetInfo(ITEM a_cItem, int SetType)
	{
		int inventoryIndex = this.GetInventoryIndex(a_cItem.m_nPosType);
		if (inventoryIndex > -1 && inventoryIndex < 7)
		{
			if (0 > a_cItem.m_nItemPos)
			{
				return;
			}
			this.m_Inventory[inventoryIndex].SetItem(a_cItem);
			if (this.CanBlinkTab(SetType))
			{
				Inventory_Dlg inventory_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.INVENTORY_DLG) as Inventory_Dlg;
				if (inventory_Dlg != null)
				{
					inventory_Dlg.SetBlinkValueStart(a_cItem.m_nPosType);
				}
			}
		}
		this.Inventory_Refresh(a_cItem.m_nPosType, a_cItem.m_nItemPos);
	}

	public void Clear()
	{
		for (int i = 0; i < 7; i++)
		{
			this.m_Inventory[i].Init();
		}
	}

	public int Get_Tab_List_Count(int ItemPosType)
	{
		int inventoryIndex = this.GetInventoryIndex(ItemPosType);
		if (inventoryIndex > -1 && inventoryIndex < 7)
		{
			return this.m_Inventory[inventoryIndex].GetItemCount();
		}
		return 0;
	}

	public void Get_Tab_List_Clear(int ItemPosType)
	{
		int inventoryIndex = this.GetInventoryIndex(ItemPosType);
		if (inventoryIndex > -1 && inventoryIndex < 7)
		{
			this.m_Inventory[inventoryIndex].Init();
		}
	}

	public int GetItemPos(int ItemPosType, int itemunique)
	{
		int inventoryIndex = this.GetInventoryIndex(ItemPosType);
		if (inventoryIndex > -1 && inventoryIndex < 7)
		{
			return this.m_Inventory[inventoryIndex].GetItemPosFromItemunique(itemunique);
		}
		return -1;
	}

	public bool ItemRemove(int ItemPosType, int ItemPos)
	{
		int inventoryIndex = this.GetInventoryIndex(ItemPosType);
		if (inventoryIndex > -1 && inventoryIndex < 7)
		{
			this.m_Inventory[inventoryIndex].RemoveItemByPos(ItemPos);
			this.Inventory_Refresh(ItemPosType, ItemPos);
			return true;
		}
		return false;
	}

	public bool ItemRemove(int _Type, int _Pos, int iNum)
	{
		if (0 >= iNum)
		{
			this.ItemRemove(_Type, _Pos);
		}
		else
		{
			this.SetItemNum(_Type, _Pos, iNum);
		}
		return true;
	}

	public ITEM GetItem(int ItemPosType, int ItemPos)
	{
		int inventoryIndex = this.GetInventoryIndex(ItemPosType);
		if (inventoryIndex > -1 && inventoryIndex < 7)
		{
			return this.m_Inventory[inventoryIndex].GetItem(ItemPos);
		}
		return null;
	}

	public ITEM GetItem(int ItemUnique)
	{
		int inventoryIndex = this.GetInventoryIndex(Protocol_Item.GetItemPosType(ItemUnique));
		if (inventoryIndex > -1 && inventoryIndex < 7)
		{
			ITEM itemByUnique = this.m_Inventory[inventoryIndex].GetItemByUnique(ItemUnique);
			if (itemByUnique != null && itemByUnique.m_nItemUnique == ItemUnique)
			{
				return itemByUnique;
			}
		}
		return null;
	}

	public ITEM GetItem(int ItemPosType, int ItemUnique, int ItemPos)
	{
		int inventoryIndex = this.GetInventoryIndex(ItemPosType);
		if (inventoryIndex > -1 && inventoryIndex < 7)
		{
			ITEM item = this.m_Inventory[inventoryIndex].GetItem(ItemPos);
			if (item != null && item.m_nItemUnique == ItemUnique)
			{
				return item;
			}
		}
		return null;
	}

	public ITEM GetItemByATB(long itematb)
	{
		for (int i = 1; i <= 4; i++)
		{
			int inventoryIndex = this.GetInventoryIndex(i);
			if (inventoryIndex > -1 && inventoryIndex < 7)
			{
				return this.m_Inventory[inventoryIndex].GetItemByATB(itematb);
			}
		}
		return null;
	}

	public ITEM GetItemFromItemID(long ItemID)
	{
		for (int i = 1; i < 8; i++)
		{
			int inventoryIndex = this.GetInventoryIndex(i);
			if (inventoryIndex > -1 && inventoryIndex < 7)
			{
				ITEM itemFromItemID = this.m_Inventory[inventoryIndex].GetItemFromItemID(ItemID);
				if (itemFromItemID != null)
				{
					return itemFromItemID;
				}
			}
		}
		return null;
	}

	public bool IsHaveItem(int nItemUnique, int nItemNum)
	{
		int itemPosFromItemUnique = this.GetItemPosFromItemUnique(nItemUnique);
		if (itemPosFromItemUnique < 0)
		{
			return false;
		}
		int itemPosType = Protocol_Item.GetItemPosType(nItemUnique);
		int num = Protocol_Item.Get_Enable_Slot_Count(itemPosType);
		if (num <= itemPosFromItemUnique)
		{
			return false;
		}
		int itemCnt = NkUserInventory.GetInstance().GetItemCnt(nItemUnique);
		return itemCnt >= nItemNum;
	}

	public int GetItemCnt(int a_lUnique)
	{
		int result = 0;
		int itemPosType = Protocol_Item.GetItemPosType(a_lUnique);
		int inventoryIndex = this.GetInventoryIndex(itemPosType);
		if (inventoryIndex > -1 && inventoryIndex < 7)
		{
			return this.m_Inventory[inventoryIndex].GetItemCount(a_lUnique);
		}
		return result;
	}

	public int GetItemCountFromATB(long itematb)
	{
		int num = 0;
		for (int i = 1; i <= 4; i++)
		{
			int inventoryIndex = this.GetInventoryIndex(i);
			if (inventoryIndex > -1 && inventoryIndex < 7)
			{
				num += this.m_Inventory[inventoryIndex].GetItemCountFromATB(itematb);
			}
		}
		return num;
	}

	public int GetItemCountFromType(int itemType)
	{
		int result = 0;
		int inventoryIndex = this.GetInventoryIndex(itemType);
		if (inventoryIndex > -1 && inventoryIndex < 7)
		{
			result = this.m_Inventory[inventoryIndex].GetItemCount();
		}
		return result;
	}

	public int GetItemPosFromItemUnique(int itemunique)
	{
		int itemPosType = Protocol_Item.GetItemPosType(itemunique);
		int inventoryIndex = this.GetInventoryIndex(itemPosType);
		if (inventoryIndex > -1 && inventoryIndex < 7)
		{
			return this.m_Inventory[inventoryIndex].GetItemPosFromItemunique(itemunique);
		}
		return -1;
	}

	public int GetFunctionItemNum(eITEM_SUPPLY_FUNCTION eType)
	{
		int num = 0;
		for (int i = 0; i < 7; i++)
		{
			if (this.m_Inventory[i] != null)
			{
				for (int j = 0; j < ItemDefine.INVENTORY_ITEMSLOT_MAX; j++)
				{
					ITEM iTEM = this.m_Inventory[i].m_kInvenItem[j];
					if (iTEM != null)
					{
						ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(iTEM.m_nItemUnique);
						if (itemInfo != null)
						{
							if (itemInfo.m_nFunctions == (byte)eType)
							{
								num += iTEM.m_nItemNum;
							}
						}
					}
				}
			}
		}
		return num;
	}

	public ITEM GetFirstFunctionItem(eITEM_SUPPLY_FUNCTION eType)
	{
		for (int i = 0; i < 7; i++)
		{
			if (this.m_Inventory[i] != null)
			{
				for (int j = 0; j < ItemDefine.INVENTORY_ITEMSLOT_MAX; j++)
				{
					ITEM iTEM = this.m_Inventory[i].m_kInvenItem[j];
					if (iTEM != null)
					{
						ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(iTEM.m_nItemUnique);
						if (itemInfo != null)
						{
							if (itemInfo.m_nFunctions == (byte)eType)
							{
								return iTEM;
							}
						}
					}
				}
			}
		}
		return null;
	}

	public List<ITEM> GetTypeItemsByInvenType(int InvenType, eITEM_TYPE eType)
	{
		List<ITEM> list = new List<ITEM>();
		if (InvenType < 0 || InvenType >= 7)
		{
			return list;
		}
		if (this.m_Inventory[InvenType] == null)
		{
			return list;
		}
		int num = 30;
		for (int i = 0; i < num; i++)
		{
			ITEM iTEM = this.m_Inventory[InvenType].m_kInvenItem[i];
			if (iTEM != null)
			{
				ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(iTEM.m_nItemUnique);
				if (itemInfo != null)
				{
					if (itemInfo.m_nItemType == (int)eType)
					{
						list.Add(iTEM);
					}
				}
			}
		}
		return list;
	}

	public List<ITEM> GetFunctionItemsByInvenType(int InvenType, eITEM_SUPPLY_FUNCTION eType)
	{
		List<ITEM> list = new List<ITEM>();
		if (InvenType < 0 || InvenType >= 7)
		{
			return list;
		}
		if (this.m_Inventory[InvenType] == null)
		{
			return list;
		}
		int num = 30;
		for (int i = 0; i < num; i++)
		{
			ITEM iTEM = this.m_Inventory[InvenType].m_kInvenItem[i];
			if (iTEM != null)
			{
				ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(iTEM.m_nItemUnique);
				if (itemInfo != null)
				{
					if (itemInfo.m_nFunctions == (byte)eType)
					{
						list.Add(iTEM);
					}
				}
			}
		}
		return list;
	}

	public List<ITEM> GetFunctionItems(eITEM_SUPPLY_FUNCTION eType)
	{
		List<ITEM> list = new List<ITEM>();
		for (int i = 0; i < 7; i++)
		{
			if (this.m_Inventory[i] != null)
			{
				for (int j = 0; j < ItemDefine.INVENTORY_ITEMSLOT_MAX; j++)
				{
					ITEM iTEM = this.m_Inventory[i].m_kInvenItem[j];
					if (iTEM != null)
					{
						ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(iTEM.m_nItemUnique);
						if (itemInfo != null)
						{
							if (itemInfo.m_nFunctions == (byte)eType)
							{
								list.Add(iTEM);
							}
						}
					}
				}
			}
		}
		return list;
	}

	public int Get_First_ItemCnt(int a_lUnique)
	{
		int itemPosType = Protocol_Item.GetItemPosType(a_lUnique);
		int inventoryIndex = this.GetInventoryIndex(itemPosType);
		if (inventoryIndex > -1 && inventoryIndex < 7)
		{
			return this.m_Inventory[inventoryIndex].Get_First_ItemCnt(a_lUnique);
		}
		return 0;
	}

	public ITEM GetFirstItemByUnique(int a_lUnique)
	{
		int itemPosType = Protocol_Item.GetItemPosType(a_lUnique);
		int inventoryIndex = this.GetInventoryIndex(itemPosType);
		if (inventoryIndex > -1 && inventoryIndex < 7)
		{
			return this.m_Inventory[inventoryIndex].GetItemFromItemunique(a_lUnique);
		}
		return null;
	}

	public ITEM GetFirstItemByUniqueLowRank(int itemUnique)
	{
		int itemPosType = Protocol_Item.GetItemPosType(itemUnique);
		int inventoryIndex = this.GetInventoryIndex(itemPosType);
		if (inventoryIndex > -1 && inventoryIndex < 7)
		{
			return this.m_Inventory[inventoryIndex].GetFristItemByUniqueLowRank(itemUnique);
		}
		return null;
	}

	public ITEM GetBatterItemByUnique(ITEM item, short level)
	{
		int itemPosType = Protocol_Item.GetItemPosType(item.m_nItemUnique);
		int inventoryIndex = this.GetInventoryIndex(itemPosType);
		if (inventoryIndex > -1 && inventoryIndex < 7)
		{
			return this.m_Inventory[inventoryIndex].GetBatterItemByUnique(item, level);
		}
		return null;
	}

	public List<ITEM> GetQuestConItem(ITEM item)
	{
		List<ITEM> list = new List<ITEM>();
		int itemPosType = Protocol_Item.GetItemPosType(item.m_nItemUnique);
		int inventoryIndex = this.GetInventoryIndex(itemPosType);
		if (inventoryIndex > -1 && inventoryIndex < 7)
		{
			ITEM itemFromItemunique = this.m_Inventory[inventoryIndex].GetItemFromItemunique(item.m_nItemUnique);
			if (itemFromItemunique != null)
			{
				list.Add(itemFromItemunique);
			}
		}
		return list;
	}

	public int SetItemNum(int a_byPosType, int a_shPosItem, int a_shItemNum)
	{
		int inventoryIndex = this.GetInventoryIndex(a_byPosType);
		if (inventoryIndex > -1 && inventoryIndex < 7)
		{
			if (a_shItemNum > 0)
			{
				this.m_Inventory[inventoryIndex].SetItemNum(a_shPosItem, a_shItemNum);
			}
			else
			{
				this.m_Inventory[inventoryIndex].RemoveItemByPos(a_shPosItem);
			}
			this.Inventory_Refresh(a_byPosType, a_shPosItem);
			return a_shItemNum;
		}
		return 0;
	}

	public int SetItemNum(int a_lUnique, int a_byPosType, int a_shPosItem, int a_shItemNum)
	{
		return this.SetItemNum(a_byPosType, a_shPosItem, a_shItemNum);
	}

	public int Get_Item_Durability(int a_nItemUnique, int a_shPosItem)
	{
		int itemPosType = Protocol_Item.GetItemPosType(a_nItemUnique);
		int inventoryIndex = this.GetInventoryIndex(itemPosType);
		if (inventoryIndex > -1 && inventoryIndex < 7)
		{
			return this.m_Inventory[inventoryIndex].Get_Item_Durability(a_nItemUnique);
		}
		return -1;
	}

	public int Get_Item_Rank(int a_nItemUnique, int a_shPosItem)
	{
		int itemPosType = Protocol_Item.GetItemPosType(a_nItemUnique);
		int inventoryIndex = this.GetInventoryIndex(itemPosType);
		if (inventoryIndex > -1 && inventoryIndex < 7)
		{
			return this.m_Inventory[inventoryIndex].Get_Item_Rank(a_nItemUnique);
		}
		return 0;
	}

	public void Set_Enalbe_Slot(byte a_byPosType, short a_shPosItem, byte a_bySlotCount)
	{
	}

	public void Init_Enalbe_Slot()
	{
		Inventory_Dlg inventory_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.INVENTORY_DLG) as Inventory_Dlg;
		if (inventory_Dlg != null)
		{
			inventory_Dlg.Item_Draw(inventory_Dlg.GetCurrentTap() + 1);
		}
	}

	public void Set_Enalbe_Slot_Sub(byte a_byPosType, byte a_bySlotCount)
	{
	}

	public void Set_Enalbe_Slot(byte a_byPosType, byte a_bySlotCount)
	{
	}

	public byte Get_Enalbe_Slot(int a_nPosType)
	{
		if (a_nPosType != 7)
		{
		}
		return 30;
	}

	public bool IsHaveEquipItem(eEQUIP_ITEM Slot, ref ITEM srcItem, NkSoldierInfo solInfo)
	{
		if (solInfo == null)
		{
			return false;
		}
		int itemPosType;
		if (Slot == eEQUIP_ITEM.EQUIP_WEAPON1)
		{
			itemPosType = solInfo.GetItemPosTypeByWeaponType();
		}
		else
		{
			itemPosType = 1;
		}
		int inventoryIndex = this.GetInventoryIndex(itemPosType);
		bool result = false;
		ITEM iTEM = null;
		ITEMINFO iTEMINFO = null;
		if (inventoryIndex > -1 && inventoryIndex < 7)
		{
			for (int i = 0; i < ItemDefine.INVENTORY_ITEMSLOT_MAX; i++)
			{
				ITEM item = this.m_Inventory[inventoryIndex].GetItem(i);
				if (item != null)
				{
					if (NrTSingleton<ItemManager>.Instance.GetItemMinLevelFromItem(item) <= (int)solInfo.GetLevel())
					{
						ITEMTYPE_INFO itemTypeInfo = NrTSingleton<ItemManager>.Instance.GetItemTypeInfo(item.m_nItemUnique);
						if (itemTypeInfo != null)
						{
							if (solInfo.IsEquipClassType(itemTypeInfo.WEAPONTYPE, itemTypeInfo.EQUIPCLASSTYPE))
							{
								ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(item.m_nItemUnique);
								int equipItemPos = (int)NrTSingleton<ItemManager>.Instance.GetEquipItemPos(item.m_nItemUnique);
								if (Slot == (eEQUIP_ITEM)equipItemPos)
								{
									if (itemInfo.m_nMinDamage != 0)
									{
										int num = 0;
										if (iTEM != null)
										{
											num = Tooltip_Dlg.GetOptionValue(iTEM.m_nOption[0], iTEMINFO.m_nMinDamage);
										}
										int optionValue = Tooltip_Dlg.GetOptionValue(item.m_nOption[0], itemInfo.m_nMinDamage);
										if (num < optionValue)
										{
											iTEM = item;
											iTEMINFO = itemInfo;
											srcItem = item;
											result = true;
										}
									}
									else if (itemInfo.m_nDefense != 0)
									{
										int num2 = 0;
										if (iTEM != null)
										{
											num2 = Tooltip_Dlg.GetOptionValue(iTEM.m_nOption[0], iTEMINFO.m_nDefense);
										}
										int optionValue2 = Tooltip_Dlg.GetOptionValue(item.m_nOption[0], itemInfo.m_nDefense);
										if (num2 < optionValue2)
										{
											iTEM = item;
											iTEMINFO = itemInfo;
											srcItem = item;
											result = true;
										}
									}
								}
							}
						}
					}
				}
			}
		}
		return result;
	}

	public int GetEmptySlot(int PosType)
	{
		if (PosType > 7)
		{
			return -1;
		}
		int num = Protocol_Item.Get_Enable_Slot_Count(PosType);
		byte b = 0;
		while ((int)b < num)
		{
			this.m_InvenSlot[(int)b] = 0;
			b += 1;
		}
		int inventoryIndex = this.GetInventoryIndex(PosType);
		if (inventoryIndex > -1 && inventoryIndex < 7)
		{
			for (int i = 0; i < ItemDefine.INVENTORY_ITEMSLOT_MAX; i++)
			{
				ITEM item = this.m_Inventory[inventoryIndex].GetItem(i);
				if (item != null)
				{
					this.m_InvenSlot[item.m_nItemPos] = 1;
				}
			}
		}
		byte b2 = 0;
		while ((int)b2 < num)
		{
			if (this.m_InvenSlot[(int)b2] == 0)
			{
				return (int)b2;
			}
			b2 += 1;
		}
		return -1;
	}

	public void GetEmptySlot(int PosType, ref List<int> _EmptySlot)
	{
		if (PosType > 7)
		{
			return;
		}
		int num = Protocol_Item.Get_Enable_Slot_Count(PosType);
		byte b = 0;
		while ((int)b < num)
		{
			this.m_InvenSlot[(int)b] = 0;
			b += 1;
		}
		int inventoryIndex = this.GetInventoryIndex(PosType);
		if (inventoryIndex > -1 && inventoryIndex < 7)
		{
			for (int i = 0; i < ItemDefine.INVENTORY_ITEMSLOT_MAX; i++)
			{
				ITEM item = this.m_Inventory[inventoryIndex].GetItem(i);
				if (item != null)
				{
					this.m_InvenSlot[item.m_nItemPos] = 1;
				}
			}
		}
		byte b2 = 0;
		while ((int)b2 < num)
		{
			if (this.m_InvenSlot[(int)b2] == 0)
			{
				_EmptySlot.Add((int)b2);
			}
			b2 += 1;
		}
	}

	public List<ITEM> GetItemFromATB(long itematb)
	{
		List<ITEM> list = new List<ITEM>();
		for (int i = 0; i < 7; i++)
		{
			for (int j = 0; j < ItemDefine.INVENTORY_ITEMSLOT_MAX; j++)
			{
				ITEM item = this.m_Inventory[i].GetItem(j);
				if (item != null)
				{
					if (item.IsValid())
					{
						if (NrTSingleton<ItemManager>.Instance.IsItemATB(item.m_nItemUnique, itematb))
						{
							list.Add(item);
						}
					}
				}
			}
		}
		return list;
	}
}
