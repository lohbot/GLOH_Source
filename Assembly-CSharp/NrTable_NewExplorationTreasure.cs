using System;
using TsLibs;

public class NrTable_NewExplorationTreasure : NrTableBase
{
	public NrTable_NewExplorationTreasure(string strFilePath) : base(strFilePath)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			NEWEXPLORATION_TREASURE nEWEXPLORATION_TREASURE = new NEWEXPLORATION_TREASURE();
			nEWEXPLORATION_TREASURE.SetData(data);
			NrTSingleton<NewExplorationManager>.Instance.AddTreasureData(nEWEXPLORATION_TREASURE);
		}
		return true;
	}
}
