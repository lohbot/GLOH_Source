using System;
using TsLibs;

public class NrTable_Item_Rank : NrTableBase
{
	public NrTable_Item_Rank(string a_strFilePath) : base(a_strFilePath)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			Item_Rank item_Rank = new Item_Rank();
			item_Rank.SetData(data);
			Item_Rank_Manager.Get_Instance().Set_Value(item_Rank.QuailtyLevel, item_Rank.ItemRank, item_Rank);
		}
		return true;
	}
}
