using System;
using TsLibs;

public class NrTable_NewExplorationSRewardTable : NrTableBase
{
	public NrTable_NewExplorationSRewardTable(string strFilePath) : base(strFilePath)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			BATTLE_NEWEXPLORATION_SREWARD bATTLE_NEWEXPLORATION_SREWARD = new BATTLE_NEWEXPLORATION_SREWARD();
			bATTLE_NEWEXPLORATION_SREWARD.SetData(data);
			for (int i = 0; i < 4; i++)
			{
				bATTLE_NEWEXPLORATION_SREWARD.m_sRewardProduct[i].m_nRewardType = NrTSingleton<BattleSReward_Manager>.Instance.GetSRewardType(bATTLE_NEWEXPLORATION_SREWARD.m_sRewardProduct[i].m_strParserRewardType);
			}
			NrTSingleton<NewExplorationManager>.Instance.AddSRewardData(bATTLE_NEWEXPLORATION_SREWARD);
		}
		return true;
	}
}
