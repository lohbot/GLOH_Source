using System;
using TsLibs;

public class NrTableMythRaidInfo : NrTableBase
{
	public NrTableMythRaidInfo() : base(CDefinePath.MYTHRAID_INFO)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<MYTHRAIDINFO_DATA>(dr);
	}
}
