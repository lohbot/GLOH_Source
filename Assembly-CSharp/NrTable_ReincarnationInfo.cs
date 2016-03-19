using System;
using TsLibs;

public class NrTable_ReincarnationInfo : NrTableBase
{
	public NrTable_ReincarnationInfo() : base(CDefinePath.REINCARNATION_URL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<ReincarnationInfo>(dr);
	}
}
