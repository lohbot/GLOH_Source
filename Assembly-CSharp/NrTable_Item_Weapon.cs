using System;
using TsLibs;

public class NrTable_Item_Weapon : NrTableBase
{
	public NrTable_Item_Weapon() : base(CDefinePath.s_strItemWeaponURL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<ITEM_WEAPON>(dr);
	}
}
