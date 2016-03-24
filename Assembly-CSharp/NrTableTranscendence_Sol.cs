using System;
using TsLibs;

public class NrTableTranscendence_Sol : NrTableBase
{
	public NrTableTranscendence_Sol() : base(CDefinePath.TRANSCENDENCE_SOL_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			TRANSCENDENCE_SOL tRANSCENDENCE_SOL = new TRANSCENDENCE_SOL();
			tRANSCENDENCE_SOL.SetData(data);
			NrTSingleton<NrTableTranscendenceManager>.Instance.AddTranscendenceSol(tRANSCENDENCE_SOL);
			NrTSingleton<NrBaseTableManager>.Instance.SetData(tRANSCENDENCE_SOL);
		}
		return true;
	}
}
