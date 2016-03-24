using System;
using TsLibs;

public class NkTableSoldierEvolutionExp : NrTableBase
{
	public NkTableSoldierEvolutionExp() : base(CDefinePath.SOLDIER_EVOLUTIONEXP_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<Evolution_EXP>(dr);
	}
}
