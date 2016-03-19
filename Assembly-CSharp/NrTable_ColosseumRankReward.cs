using System;
using TsLibs;

public class NrTable_ColosseumRankReward : NrTableBase
{
	public NrTable_ColosseumRankReward() : base(CDefinePath.s_strColosseumRankRewardURL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			COLOSSEUM_RANK_REWARD cOLOSSEUM_RANK_REWARD = new COLOSSEUM_RANK_REWARD();
			cOLOSSEUM_RANK_REWARD.SetData(data);
			NrTSingleton<NrTable_ColosseumRankReward_Manager>.Instance.AddRewardInfo(cOLOSSEUM_RANK_REWARD);
		}
		return true;
	}
}
