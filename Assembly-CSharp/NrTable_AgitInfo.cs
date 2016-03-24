using System;
using TsLibs;

public class NrTable_AgitInfo : NrTableBase
{
	public NrTable_AgitInfo() : base(CDefinePath.AGIT_INFO_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<AgitInfoData>(dr);
	}
}
