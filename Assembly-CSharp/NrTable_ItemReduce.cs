using System;
using TsLibs;

public class NrTable_ItemReduce : NrTableBase
{
	public NrTable_ItemReduce() : base(CDefinePath.ITEM_REDUCE_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<ItemReduceInfo>(dr);
	}
}
