using System;
using System.Collections.Generic;

public class ITEM_SELL_Manager : NrTSingleton<ITEM_SELL_Manager>
{
	private Dictionary<int, Dictionary<int, ITEM_SELL>> mHash_ItemSellData;

	private NkValueParse<int> m_kItemRankType;

	private ITEM_SELL_Manager()
	{
		this.mHash_ItemSellData = new Dictionary<int, Dictionary<int, ITEM_SELL>>();
		this.m_kItemRankType = new NkValueParse<int>();
		this.SetParaDataCode();
	}

	public void SetParaDataCode()
	{
		this.m_kItemRankType.InsertCodeValue("D", 1);
		this.m_kItemRankType.InsertCodeValue("C", 2);
		this.m_kItemRankType.InsertCodeValue("B", 3);
		this.m_kItemRankType.InsertCodeValue("A", 4);
		this.m_kItemRankType.InsertCodeValue("S", 5);
		this.m_kItemRankType.InsertCodeValue("SS", 6);
	}

	public void SetItemSellData(ITEM_SELL SellData)
	{
		if (!this.mHash_ItemSellData.ContainsKey(SellData.nGroupUnique))
		{
			this.mHash_ItemSellData.Add(SellData.nGroupUnique, new Dictionary<int, ITEM_SELL>());
		}
		if (!this.mHash_ItemSellData[SellData.nGroupUnique].ContainsKey(SellData.nItemMakeRank))
		{
			this.mHash_ItemSellData[SellData.nGroupUnique].Add(SellData.nItemMakeRank, SellData);
		}
	}

	public int GetRankType(string RankType)
	{
		return this.m_kItemRankType.GetValue(RankType);
	}

	public ITEM_SELL GetItemSellData(int GroupUnique, int ItemMakeRank)
	{
		if (this.mHash_ItemSellData.ContainsKey(GroupUnique))
		{
			foreach (KeyValuePair<int, ITEM_SELL> current in this.mHash_ItemSellData[GroupUnique])
			{
				int key = current.Key;
				ITEM_SELL value = current.Value;
				if (key == ItemMakeRank)
				{
					return value;
				}
			}
		}
		return null;
	}
}
