using System;
using TsLibs;

public class NrTable_Item_Material : NrTableBase
{
	public NrTable_Item_Material() : base(CDefinePath.s_strItemMaterialURL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<ITEM_MATERIAL>(dr);
	}
}
