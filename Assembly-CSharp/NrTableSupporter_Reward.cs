using System;
using TsLibs;

public class NrTableSupporter_Reward : NrTableBase
{
	public NrTableSupporter_Reward() : base(CDefinePath.SUPPORTER_REWARD_TABLE_PATH)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<SUPPORTER_REWARD>(dr);
	}
}
