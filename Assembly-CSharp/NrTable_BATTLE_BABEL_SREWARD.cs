using System;
using TsLibs;

public class NrTable_BATTLE_BABEL_SREWARD : NrTableBase
{
	public NrTable_BATTLE_BABEL_SREWARD() : base(CDefinePath.BATTLE_BABEL_SREWARD_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			BATTLE_BABEL_SREWARD bATTLE_BABEL_SREWARD = new BATTLE_BABEL_SREWARD();
			bATTLE_BABEL_SREWARD.SetData(data);
			for (int i = 0; i < 4; i++)
			{
				bATTLE_BABEL_SREWARD.m_sRewardProduct[i].m_nRewardType = NrTSingleton<BattleSReward_Manager>.Instance.GetSRewardType(bATTLE_BABEL_SREWARD.m_sRewardProduct[i].m_strParserRewardType);
			}
			NrTSingleton<BattleSReward_Manager>.Instance.SetBabelSpecialReward(bATTLE_BABEL_SREWARD);
		}
		return true;
	}
}
