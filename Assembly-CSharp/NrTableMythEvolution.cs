using System;
using TsLibs;

public class NrTableMythEvolution : NrTableBase
{
	public NrTableMythEvolution() : base(CDefinePath.MYTH_EVOLUTION_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			MYTH_EVOLUTION mYTH_EVOLUTION = new MYTH_EVOLUTION();
			mYTH_EVOLUTION.SetData(data);
			NrTSingleton<NrTableMyth_EvolutionManager>.Instance.AddMyth_Evolution(mYTH_EVOLUTION);
			NrTSingleton<NrBaseTableManager>.Instance.SetData(mYTH_EVOLUTION);
		}
		return true;
	}
}
