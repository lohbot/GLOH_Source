using System;
using TsLibs;

public class NrTable_Item_Armor : NrTableBase
{
	public NrTable_Item_Armor() : base(CDefinePath.s_strItemArmorURL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<ITEM_ARMOR>(dr);
	}
}
