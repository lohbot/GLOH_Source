using System;
using TsLibs;

public class NrTableCharKindNPCInfo : NrTableBase
{
	public NrTableCharKindNPCInfo() : base(CDefinePath.CHARKIND_NPCINFO_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<CHARKIND_NPCINFO>(dr);
	}
}
