using System;
using TsLibs;

public class NrTable_ITEMEVOLUTION : NrTableBase
{
	public NrTable_ITEMEVOLUTION() : base(CDefinePath.s_strItemEvolutionURL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			ITEMEVOLUTION iTEMEVOLUTION = new ITEMEVOLUTION();
			iTEMEVOLUTION.SetData(data);
			NrTSingleton<ITEMEVOLUTION_Manager>.Instance.SetItemEvolutioneData(iTEMEVOLUTION);
		}
		return true;
	}
}
