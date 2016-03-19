using System;
using TsLibs;

public class NrTable_Item_Second_Equip : NrTableBase
{
	public NrTable_Item_Second_Equip() : base(CDefinePath.s_strItemSecondEquipURL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<ITEM_SECONDEQUIP>(dr);
	}
}
