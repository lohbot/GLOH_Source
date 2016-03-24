using System;
using TsLibs;

public class NrTableMythRaid_Clear_Reward : NrTableBase
{
	public NrTableMythRaid_Clear_Reward() : base(CDefinePath.MYTHRAID_CLEAR_REWARD)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<MYTHRAID_CLEAR_REWARD_INFO>(dr);
	}
}
