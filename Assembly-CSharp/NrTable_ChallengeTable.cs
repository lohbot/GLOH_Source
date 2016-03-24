using System;
using TsLibs;

public class NrTable_ChallengeTable : NrTableBase
{
	public NrTable_ChallengeTable(string strFilePath) : base(strFilePath)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			ChallengeTable challengeTable = new ChallengeTable();
			challengeTable.SetData(data);
			if (0 < challengeTable.m_nType)
			{
				NrTSingleton<ChallengeManager>.Instance.AddChallengeTable(challengeTable);
				if (0 < challengeTable.m_nSequence)
				{
					NrTSingleton<ChallengeManager>.Instance.AddSequenceChallengeTable(challengeTable);
				}
			}
		}
		NrTSingleton<ChallengeManager>.Instance.CalcTotalRewardCount();
		return true;
	}
}
