using System;
using TsLibs;

public class NrTable_BATTLE_SREWARD : NrTableBase
{
	public NrTable_BATTLE_SREWARD() : base(CDefinePath.BATTLE_SREWARD_URL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			BATTLE_SREWARD bATTLE_SREWARD = new BATTLE_SREWARD();
			bATTLE_SREWARD.SetData(data);
			for (int i = 0; i < 4; i++)
			{
				bATTLE_SREWARD.m_sRewardProduct[i].m_nRewardType = NrTSingleton<BattleSReward_Manager>.Instance.GetSRewardType(bATTLE_SREWARD.m_sRewardProduct[i].m_strParserRewardType);
			}
			NrTSingleton<BattleSReward_Manager>.Instance.SetBattleSRewardData(bATTLE_SREWARD);
		}
		return true;
	}
}
