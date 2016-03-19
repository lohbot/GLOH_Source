using System;
using TsLibs;

public class NrTableTranscendence_Cost : NrTableBase
{
	public NrTableTranscendence_Cost() : base(CDefinePath.TRANSCENDENCE_COST_URL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			TRANSCENDENCE_COST tRANSCENDENCE_COST = new TRANSCENDENCE_COST();
			tRANSCENDENCE_COST.SetData(data);
			NrTSingleton<NrTableTranscendenceManager>.Instance.AddTranscendenceCost(tRANSCENDENCE_COST);
			NrTSingleton<NrBaseTableManager>.Instance.SetData(tRANSCENDENCE_COST);
		}
		return true;
	}
}
