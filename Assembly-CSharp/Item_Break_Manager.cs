using System;
using System.Collections.Generic;

public class Item_Break_Manager : NrTSingleton<Item_Break_Manager>
{
	private Dictionary<int, ITEM_BREAK[]> m_calItemBreak = new Dictionary<int, ITEM_BREAK[]>();

	private Item_Break_Manager()
	{
	}

	public void Set_Value(int i32GroupUnique, int rank, ITEM_BREAK a_cValue)
	{
		if (rank >= 7)
		{
			return;
		}
		if (!this.m_calItemBreak.ContainsKey(i32GroupUnique))
		{
			ITEM_BREAK[] value = new ITEM_BREAK[7];
			this.m_calItemBreak.Add(i32GroupUnique, value);
		}
		this.m_calItemBreak[i32GroupUnique][rank] = a_cValue;
	}

	public ITEM_BREAK Get_RankData(int i32GroupUnique, int rank = 6)
	{
		if (rank >= 7)
		{
			return null;
		}
		if (this.m_calItemBreak.ContainsKey(i32GroupUnique))
		{
			return this.m_calItemBreak[i32GroupUnique][rank];
		}
		if (this.m_calItemBreak.ContainsKey(0))
		{
			return this.m_calItemBreak[0][rank];
		}
		return null;
	}

	public ITEM_BREAK[] Get_Collection(int i32GroupUnique)
	{
		if (this.m_calItemBreak.ContainsKey(i32GroupUnique))
		{
			return this.m_calItemBreak[i32GroupUnique];
		}
		return null;
	}
}
