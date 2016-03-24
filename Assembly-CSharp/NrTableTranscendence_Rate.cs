using System;
using TsLibs;

public class NrTableTranscendence_Rate : NrTableBase
{
	public NrTableTranscendence_Rate() : base(CDefinePath.TRANSCENDENCE_RATE_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			TRANSCENDENCE_RATE tRANSCENDENCE_RATE = new TRANSCENDENCE_RATE();
			tRANSCENDENCE_RATE.SetData(data);
			NrTSingleton<NrTableTranscendenceManager>.Instance.AddTranscendenceRate(tRANSCENDENCE_RATE);
			NrTSingleton<NrBaseTableManager>.Instance.SetData(tRANSCENDENCE_RATE);
		}
		return true;
	}
}
