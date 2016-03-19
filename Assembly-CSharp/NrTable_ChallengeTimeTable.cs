using System;
using TsLibs;

public class NrTable_ChallengeTimeTable : NrTableBase
{
	public NrTable_ChallengeTimeTable(string strFilePath) : base(strFilePath, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			ChallengeTimeTable challengeTimeTable = new ChallengeTimeTable();
			challengeTimeTable.SetData(data);
			NrTSingleton<ChallengeManager>.Instance.AddChallengeTimeTable(challengeTimeTable);
		}
		return true;
	}
}
