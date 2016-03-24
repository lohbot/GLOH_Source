using System;
using TsLibs;

public class NrTable_Item_Break : NrTableBase
{
	public NrTable_Item_Break() : base(CDefinePath.ITEM_BREAK_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			ITEM_BREAK iTEM_BREAK = new ITEM_BREAK();
			iTEM_BREAK.SetData(data);
			NrTSingleton<Item_Break_Manager>.Instance.Set_Value(iTEM_BREAK.i32GroupUnique, iTEM_BREAK.i32ItemMakeRank, iTEM_BREAK);
		}
		return true;
	}
}
