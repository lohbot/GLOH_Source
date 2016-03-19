using System;
using TsLibs;

public class NrLEVEL_EXPTable : NrTableBase
{
	public NrLEVEL_EXPTable(string strFilePath) : base(strFilePath)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		NrTSingleton<NkLevelManager>.Instance.Init();
		foreach (TsDataReader.Row data in dr)
		{
			LEVEL_EXP lEVEL_EXP = new LEVEL_EXP();
			lEVEL_EXP.SetData(data);
			NrTSingleton<NkLevelManager>.Instance.Add(lEVEL_EXP);
		}
		return true;
	}
}
