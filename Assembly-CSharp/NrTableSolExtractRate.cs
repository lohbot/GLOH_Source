using System;
using TsLibs;

public class NrTableSolExtractRate : NrTableBase
{
	public NrTableSolExtractRate() : base(CDefinePath.SOL_EXTRACTRATE_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			EXTRACT_RATE eXTRACT_RATE = new EXTRACT_RATE();
			eXTRACT_RATE.SetData(data);
			NrTSingleton<NrSolExtractRateManager>.Instance.AddData(eXTRACT_RATE);
		}
		return true;
	}
}
