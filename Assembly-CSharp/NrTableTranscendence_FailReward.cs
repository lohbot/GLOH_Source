using System;
using TsLibs;

public class NrTableTranscendence_FailReward : NrTableBase
{
	public NrTableTranscendence_FailReward() : base(CDefinePath.TRANSCENDENCE_FAILREWARD_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			TRANSCENDENCE_FAILREWARD tRANSCENDENCE_FAILREWARD = new TRANSCENDENCE_FAILREWARD();
			tRANSCENDENCE_FAILREWARD.SetData(data);
			NrTSingleton<NrTableTranscendenceManager>.Instance.AddTranscendenceFailReward(tRANSCENDENCE_FAILREWARD);
			NrTSingleton<NrBaseTableManager>.Instance.SetData(tRANSCENDENCE_FAILREWARD);
		}
		return true;
	}
}
