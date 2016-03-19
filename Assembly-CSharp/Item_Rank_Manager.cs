using System;
using System.Collections.Generic;

public class Item_Rank_Manager
{
	private const int N_ARRAY_COUNT = 101;

	private static Item_Rank_Manager s_cInstance;

	private Dictionary<int, Item_Rank[]> m_calItemRank = new Dictionary<int, Item_Rank[]>();

	public static Item_Rank_Manager Get_Instance()
	{
		if (Item_Rank_Manager.s_cInstance == null)
		{
			Item_Rank_Manager.s_cInstance = new Item_Rank_Manager();
		}
		return Item_Rank_Manager.s_cInstance;
	}

	public Item_Rank Get_RankData(int QualityLevel, int rank)
	{
		if (rank > 101)
		{
			return null;
		}
		if (this.m_calItemRank.ContainsKey(QualityLevel))
		{
			return this.m_calItemRank[QualityLevel][rank];
		}
		if (this.m_calItemRank.ContainsKey(0))
		{
			return this.m_calItemRank[0][rank];
		}
		return null;
	}

	public Item_Rank[] Get_Collection(int quailtyLevel)
	{
		if (this.m_calItemRank.ContainsKey(quailtyLevel))
		{
			return this.m_calItemRank[quailtyLevel];
		}
		return null;
	}

	public void Set_Value(int QualityLevel, int rank, Item_Rank a_cValue)
	{
		if (rank > 101)
		{
			return;
		}
		if (!this.m_calItemRank.ContainsKey(QualityLevel))
		{
			Item_Rank[] value = new Item_Rank[101];
			this.m_calItemRank.Add(QualityLevel, value);
		}
		this.m_calItemRank[QualityLevel][rank] = a_cValue;
	}
}
