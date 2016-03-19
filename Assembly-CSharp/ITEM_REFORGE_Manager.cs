using System;
using System.Collections.Generic;

public class ITEM_REFORGE_Manager : NrTSingleton<ITEM_REFORGE_Manager>
{
	private Dictionary<int, Dictionary<int, ITEM_REFORGE>> mHash_ItemReforgeData;

	private NkValueParse<int> m_kItemRankType;

	private ITEM_REFORGE_Manager()
	{
		this.mHash_ItemReforgeData = new Dictionary<int, Dictionary<int, ITEM_REFORGE>>();
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

	public void SetItemReforgeData(ITEM_REFORGE ReforgeData)
	{
		if (!this.mHash_ItemReforgeData.ContainsKey(ReforgeData.nGroupUnique))
		{
			this.mHash_ItemReforgeData.Add(ReforgeData.nGroupUnique, new Dictionary<int, ITEM_REFORGE>());
		}
		if (!this.mHash_ItemReforgeData[ReforgeData.nGroupUnique].ContainsKey(ReforgeData.nItemMakeRank))
		{
			this.mHash_ItemReforgeData[ReforgeData.nGroupUnique].Add(ReforgeData.nItemMakeRank, ReforgeData);
		}
	}

	public int GetRankType(string RankType)
	{
		return this.m_kItemRankType.GetValue(RankType);
	}

	public ITEM_REFORGE GetItemReforgeData(int GroupUnique, int ItemMakeRank)
	{
		if (this.mHash_ItemReforgeData.ContainsKey(GroupUnique))
		{
			foreach (KeyValuePair<int, ITEM_REFORGE> current in this.mHash_ItemReforgeData[GroupUnique])
			{
				int key = current.Key;
				ITEM_REFORGE value = current.Value;
				if (key == ItemMakeRank)
				{
					return value;
				}
			}
		}
		return null;
	}
}
