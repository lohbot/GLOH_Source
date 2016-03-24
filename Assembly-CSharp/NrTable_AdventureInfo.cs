using System;
using TsLibs;

public class NrTable_AdventureInfo : NrTableBase
{
	public NrTable_AdventureInfo() : base(CDefinePath.ADVENTURE_INFO_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<TableData_AdventureInfo>(dr);
	}
}
