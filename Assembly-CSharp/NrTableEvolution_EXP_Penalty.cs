using System;
using TsLibs;

public class NrTableEvolution_EXP_Penalty : NrTableBase
{
	public NrTableEvolution_EXP_Penalty() : base(CDefinePath.SEASON_EXP_PENALTY_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			Evolution_EXP_Penalty evolution_EXP_Penalty = new Evolution_EXP_Penalty();
			evolution_EXP_Penalty.SetData(data);
			NrTSingleton<Evolution_EXP_Penalty_Manager>.Instance.Add(evolution_EXP_Penalty);
		}
		return true;
	}
}
