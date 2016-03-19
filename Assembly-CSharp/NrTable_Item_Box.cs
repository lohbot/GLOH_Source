using System;
using TsLibs;

public class NrTable_Item_Box : NrTableBase
{
	public NrTable_Item_Box() : base(CDefinePath.s_strItemBoxURL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<ITEM_BOX>(dr);
	}
}
