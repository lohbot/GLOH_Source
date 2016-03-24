using System;
using TsLibs;

public class NrTable_AgitNPC : NrTableBase
{
	public NrTable_AgitNPC() : base(CDefinePath.AGIT_NPC_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<AgitNPCData>(dr);
	}
}
