using GAME;
using System;
using System.Collections.Generic;

public static class Protocol_Market
{
	public const int N_SELL_MAX = 10;

	public static List<MARKETITEM_INFO> s_lsBuyItem = new List<MARKETITEM_INFO>();

	public static List<MARKETITEM_INFO> s_lsSellItem = new List<MARKETITEM_INFO>();

	public static List<MARKETITEM_INFO> s_lsPriceItem = new List<MARKETITEM_INFO>();

	public static bool Is_Same_Sell_Item(ITEM a_cItem)
	{
		int nItemUnique = a_cItem.m_nItemUnique;
		if (Protocol_Item.Is_EquipItem(nItemUnique))
		{
			return false;
		}
		for (int i = 0; i < Protocol_Market.s_lsSellItem.Count; i++)
		{
			if (Protocol_Market.s_lsSellItem[i].m_cItem.m_nItemUnique == nItemUnique)
			{
				return true;
			}
		}
		return false;
	}
}
