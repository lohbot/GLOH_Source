using System;
using TsLibs;

public class NrTable_Item_Quest : NrTableBase
{
	public NrTable_Item_Quest() : base(CDefinePath.s_strItemQuestURL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<ITEM_QUEST>(dr);
	}
}
