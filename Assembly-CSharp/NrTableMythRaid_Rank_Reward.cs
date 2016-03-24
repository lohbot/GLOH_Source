using System;
using TsLibs;

public class NrTableMythRaid_Rank_Reward : NrTableBase
{
	public NrTableMythRaid_Rank_Reward() : base(CDefinePath.MYTHRAID_RANK_REWARD)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<MYTHRAID_RANK_REWARD_INFO>(dr);
	}
}
