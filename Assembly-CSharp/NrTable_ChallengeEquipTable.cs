using System;
using TsLibs;

public class NrTable_ChallengeEquipTable : NrTableBase
{
	public NrTable_ChallengeEquipTable(string strFilePath) : base(strFilePath)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			ChallengeEquipTable challengeEquipTable = new ChallengeEquipTable();
			challengeEquipTable.SetData(data);
			if (0 < challengeEquipTable.m_ChallengeIdx)
			{
				NrTSingleton<ChallengeManager>.Instance.AddChallengeEquipTable(challengeEquipTable);
			}
		}
		return true;
	}
}
