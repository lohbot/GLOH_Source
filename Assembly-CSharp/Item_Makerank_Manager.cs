using System;
using System.Collections.Generic;

public class Item_Makerank_Manager : NrTSingleton<Item_Makerank_Manager>
{
	private List<MAKE_RANK> mHash_ItemMakeRankData;

	private Item_Makerank_Manager()
	{
		this.mHash_ItemMakeRankData = new List<MAKE_RANK>();
	}

	public void SetItemMakeRankData(MAKE_RANK makeData)
	{
		this.mHash_ItemMakeRankData.Add(makeData);
	}

	public short GetItemAblility(byte Rank)
	{
		for (int i = 0; i < this.mHash_ItemMakeRankData.Count; i++)
		{
			if (this.mHash_ItemMakeRankData[i].m_byRank == Rank)
			{
				return this.mHash_ItemMakeRankData[i].m_shAblility;
			}
		}
		return this.mHash_ItemMakeRankData[this.mHash_ItemMakeRankData.Count - 1].m_shAblility;
	}
}
