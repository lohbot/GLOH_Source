using System;
using TsLibs;

public class NrTable_ITEM_BOX_GROUP_DATA : NrTableBase
{
	public NrTable_ITEM_BOX_GROUP_DATA() : base(CDefinePath.s_strItemBoxGroupURL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<ITEM_BOX_GROUP_DATA>(dr);
	}
}
