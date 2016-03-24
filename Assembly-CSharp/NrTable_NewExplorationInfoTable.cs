using System;
using TsLibs;

public class NrTable_NewExplorationInfoTable : NrTableBase
{
	public NrTable_NewExplorationInfoTable(string strFilePath) : base(strFilePath)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			NEWEXPLORATION_DATA nEWEXPLORATION_DATA = new NEWEXPLORATION_DATA();
			string charcode = nEWEXPLORATION_DATA.SetData(data);
			nEWEXPLORATION_DATA.i32BossCharKind = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(charcode);
			NrTSingleton<NewExplorationManager>.Instance.AddData(nEWEXPLORATION_DATA);
		}
		return true;
	}
}
