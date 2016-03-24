using System;
using TsLibs;

public class NrTableRecommend_Reward : NrTableBase
{
	public NrTableRecommend_Reward() : base(CDefinePath.RECOMMEND_REWARD_TABLE_PATH)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<RECOMMEND_REWARD>(dr);
	}
}
