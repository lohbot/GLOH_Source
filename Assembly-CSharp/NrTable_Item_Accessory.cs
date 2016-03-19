using System;
using TsLibs;

public class NrTable_Item_Accessory : NrTableBase
{
	public NrTable_Item_Accessory() : base(CDefinePath.s_strItemAccessoryURL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<ITEM_ACCESSORY>(dr);
	}
}
