using System;
using TsLibs;

public class NrTable_Item_Supplies : NrTableBase
{
	public NrTable_Item_Supplies() : base(CDefinePath.s_strItemSuppliesURL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<ITEM_SUPPLIES>(dr);
	}
}
