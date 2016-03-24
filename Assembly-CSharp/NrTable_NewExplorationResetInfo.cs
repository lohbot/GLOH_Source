using System;
using TsLibs;

public class NrTable_NewExplorationResetInfo : NrTableBase
{
	public NrTable_NewExplorationResetInfo(string strFilePath) : base(strFilePath)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			NEWEXPLORATION_RESET_INFO nEWEXPLORATION_RESET_INFO = new NEWEXPLORATION_RESET_INFO();
			nEWEXPLORATION_RESET_INFO.SetData(data);
			NrTSingleton<NewExplorationManager>.Instance.AddResetInfoData(nEWEXPLORATION_RESET_INFO);
		}
		return true;
	}
}
