using System;
using TsLibs;

public class NrTable_NewExplorationRankReward : NrTableBase
{
	public NrTable_NewExplorationRankReward(string strFilePath) : base(strFilePath)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			NEWEXPLORATION_RANK_REWARD nEWEXPLORATION_RANK_REWARD = new NEWEXPLORATION_RANK_REWARD();
			nEWEXPLORATION_RANK_REWARD.SetData(data);
			NrTSingleton<NewExplorationManager>.Instance.AddRankRewardData(nEWEXPLORATION_RANK_REWARD);
		}
		NrTSingleton<NewExplorationManager>.Instance.SortRankRewardData();
		return true;
	}
}
