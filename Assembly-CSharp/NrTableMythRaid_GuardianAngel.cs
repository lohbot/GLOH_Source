using System;
using TsLibs;

public class NrTableMythRaid_GuardianAngel : NrTableBase
{
	public NrTableMythRaid_GuardianAngel() : base(CDefinePath.MYTHRAID_GuardianAngel)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<MYTHRAID_GUARDIANANGEL_INFO>(dr);
	}
}
