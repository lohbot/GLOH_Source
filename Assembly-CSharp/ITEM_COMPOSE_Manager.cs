using System;
using System.Collections.Generic;
using UnityEngine;

public class ITEM_COMPOSE_Manager : NrTSingleton<ITEM_COMPOSE_Manager>
{
	public struct Compose_Material_Item
	{
		public int m_nItemUnique;

		public int m_shItemNumber;

		public string m_strItemName;

		public UIBaseInfoLoader m_t2ItemIcon;
	}

	private SortedDictionary<long, ITEM_COMPOSE> m_sdCollection;

	private ITEM_COMPOSE_Manager()
	{
		this.m_sdCollection = new SortedDictionary<long, ITEM_COMPOSE>();
	}

	public int Get_Count()
	{
		return this.m_sdCollection.Count;
	}

	public void Set_Value(ITEM_COMPOSE a_cValue)
	{
		if (!this.m_sdCollection.ContainsKey((long)a_cValue.m_nComposeProductionID))
		{
			this.m_sdCollection.Add((long)a_cValue.m_nComposeProductionID, a_cValue);
		}
		else
		{
			Debug.LogWarning("ITEM_COMPOSE Set_Value is already set. ID = " + a_cValue.m_nComposeProductionID.ToString());
		}
	}

	public int GetComposeItemUnqiue(int a_lProductionID)
	{
		if (this.m_sdCollection.ContainsKey((long)a_lProductionID))
		{
			return this.m_sdCollection[(long)a_lProductionID].m_nComposeItemUnique;
		}
		return 0;
	}

	public string GetComposeItemName(int a_lProductionID)
	{
		if (this.m_sdCollection.ContainsKey((long)a_lProductionID))
		{
			return NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_sdCollection[(long)a_lProductionID].m_nComposeItemUnique);
		}
		return string.Empty;
	}

	public ITEM_COMPOSE GetComposeItemByID(int a_lProductionID)
	{
		if (this.m_sdCollection.ContainsKey((long)a_lProductionID))
		{
			return this.m_sdCollection[(long)a_lProductionID];
		}
		return null;
	}

	public ITEM_COMPOSE GetComposeItemByNPCKIND(int a_lNPCKIND)
	{
		foreach (ITEM_COMPOSE current in this.m_sdCollection.Values)
		{
			if (current.m_nComposeNpcKind == a_lNPCKIND)
			{
				return current;
			}
		}
		return null;
	}

	public ITEM_COMPOSE_Manager.Compose_Material_Item[] ComposeItem_MaterialList(ITEM_COMPOSE a_sInfo)
	{
		List<ITEM_COMPOSE_Manager.Compose_Material_Item> list = new List<ITEM_COMPOSE_Manager.Compose_Material_Item>();
		for (int i = 0; i < 10; i++)
		{
			int num = a_sInfo.m_nMaterialItemUnique[i];
			int num2 = a_sInfo.m_nMaterialItemNum[i];
			if (num > 0 && num2 > 0)
			{
				ITEM_COMPOSE_Manager.Compose_Material_Item item;
				item.m_nItemUnique = num;
				item.m_shItemNumber = num2;
				item.m_strItemName = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(num);
				item.m_t2ItemIcon = NrTSingleton<ItemManager>.Instance.GetItemTexture(num);
				list.Add(item);
			}
		}
		return list.ToArray();
	}

	public int CanComposeItemNum(ITEM_COMPOSE a_cProduction, int a_nComposeItemNum)
	{
		ITEM_COMPOSE_Manager.Compose_Material_Item[] array = this.ComposeItem_MaterialList(a_cProduction);
		int num = 99999;
		int num2 = 0;
		if (a_nComposeItemNum == -1)
		{
			a_nComposeItemNum = num;
		}
		for (int i = 0; i < array.Length; i++)
		{
			int nItemUnique = array[i].m_nItemUnique;
			int itemCnt = NkUserInventory.GetInstance().GetItemCnt(nItemUnique);
			int num3 = array[i].m_shItemNumber * a_nComposeItemNum;
			if (num3 > itemCnt)
			{
				if (itemCnt > 0 && num3 > 0)
				{
					num2 = itemCnt / array[i].m_shItemNumber;
				}
				else if (itemCnt == 0)
				{
					num2 = 0;
				}
			}
			else
			{
				num2 = a_nComposeItemNum;
			}
			if (num > num2)
			{
				num = num2;
			}
		}
		return num;
	}

	public bool CheckComposeItem(ITEM_COMPOSE a_cProduction, int a_nMaterialItemUnique)
	{
		ITEM_COMPOSE_Manager.Compose_Material_Item[] array = this.ComposeItem_MaterialList(a_cProduction);
		bool result = false;
		for (int i = 0; i < array.Length; i++)
		{
			int nItemUnique = array[i].m_nItemUnique;
			if (nItemUnique > 0 && a_nMaterialItemUnique > 0 && nItemUnique == a_nMaterialItemUnique)
			{
				result = true;
			}
		}
		return result;
	}
}
