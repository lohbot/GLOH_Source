using System;
using TsLibs;

public class NrTable_SolWarehouseInfo : NrTableBase
{
	public NrTable_SolWarehouseInfo() : base(CDefinePath.SOLWAREHOUSE_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<SolWarehouseInfo>(dr);
	}
}
