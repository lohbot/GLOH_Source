using PROTOCOL;
using System;
using System.Collections.Generic;

public class ItemMallPoPupShopManager : NrTSingleton<ItemMallPoPupShopManager>
{
	public enum ePoPupShop_Type
	{
		NONE,
		Login,
		ShopOpen,
		RecruitOpen,
		InvenOpen
	}

	private SortedDictionary<long, List<ITEMMALL_POPUPSHOP>> m_OriginalPoPupData;

	private SortedDictionary<long, List<POPUPSHOP_DATA>> m_ServerPoPupData;

	private ItemMallPoPupShopManager()
	{
		this.m_OriginalPoPupData = new SortedDictionary<long, List<ITEMMALL_POPUPSHOP>>();
		this.m_ServerPoPupData = new SortedDictionary<long, List<POPUPSHOP_DATA>>();
	}

	public void Set_Value(ITEMMALL_POPUPSHOP a_cValue)
	{
		if (a_cValue == null)
		{
			return;
		}
		if (this.m_OriginalPoPupData.ContainsKey((long)a_cValue.m_Idx))
		{
			this.m_OriginalPoPupData[(long)a_cValue.m_Idx].Add(a_cValue);
		}
		else
		{
			this.m_OriginalPoPupData.Add((long)a_cValue.m_Idx, new List<ITEMMALL_POPUPSHOP>());
			this.m_OriginalPoPupData[(long)a_cValue.m_Idx].Add(a_cValue);
		}
	}

	public ITEMMALL_POPUPSHOP GetOriginalItem(long Index)
	{
		foreach (List<ITEMMALL_POPUPSHOP> current in this.m_OriginalPoPupData.Values)
		{
			for (int i = 0; i < current.Count; i++)
			{
				if ((long)current[i].m_Idx == Index)
				{
					return current[i];
				}
			}
		}
		return null;
	}

	public ITEMMALL_POPUPSHOP GetPoPupShop_ShopIdx(long ShopIdx)
	{
		if (this.m_OriginalPoPupData == null)
		{
			return null;
		}
		foreach (List<ITEMMALL_POPUPSHOP> current in this.m_OriginalPoPupData.Values)
		{
			if (current != null)
			{
				foreach (ITEMMALL_POPUPSHOP current2 in current)
				{
					if (current2 != null)
					{
						if (current2.m_nShopIDX == ShopIdx)
						{
							return current2;
						}
					}
				}
			}
		}
		return null;
	}

	public ITEMMALL_POPUPSHOP GetPoPupShop_AfterItemBuyLimit(long AfterProdeuctIdx)
	{
		if (this.m_OriginalPoPupData == null)
		{
			return null;
		}
		foreach (List<ITEMMALL_POPUPSHOP> current in this.m_OriginalPoPupData.Values)
		{
			if (current != null)
			{
				foreach (ITEMMALL_POPUPSHOP current2 in current)
				{
					if (current2 != null)
					{
						if ((long)current2.m_Idx == AfterProdeuctIdx && current2.m_Idx != 0)
						{
							return current2;
						}
					}
				}
			}
		}
		return null;
	}

	public ITEMMALL_POPUPSHOP GetAtbToIDX(ItemMallPoPupShopManager.ePoPupShop_Type strAtb)
	{
		if (this.m_OriginalPoPupData == null)
		{
			return null;
		}
		foreach (List<ITEMMALL_POPUPSHOP> current in this.m_OriginalPoPupData.Values)
		{
			if (current != null)
			{
				foreach (ITEMMALL_POPUPSHOP current2 in current)
				{
					if (current2 != null)
					{
						if (current2.m_nATB == (int)strAtb)
						{
							return current2;
						}
					}
				}
			}
		}
		return null;
	}

	public void Set_ServerValue(POPUPSHOP_DATA a_cValue)
	{
		if (a_cValue == null)
		{
			return;
		}
		if (this.m_ServerPoPupData.ContainsKey((long)a_cValue.i32Idx))
		{
			this.m_ServerPoPupData[(long)a_cValue.i32Idx].Add(a_cValue);
		}
		else
		{
			this.m_ServerPoPupData.Add((long)a_cValue.i32Idx, new List<POPUPSHOP_DATA>());
			this.m_ServerPoPupData[(long)a_cValue.i32Idx].Add(a_cValue);
		}
	}

	public POPUPSHOP_DATA GetServerItem(long Index)
	{
		foreach (List<POPUPSHOP_DATA> current in this.m_ServerPoPupData.Values)
		{
			for (int i = 0; i < current.Count; i++)
			{
				if ((long)current[i].i32Idx == Index)
				{
					return current[i];
				}
			}
		}
		return null;
	}
}
