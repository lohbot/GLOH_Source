using System;
using TsLibs;

public class NrTable_PlunderSupportGold : NrTableBase
{
	public NrTable_PlunderSupportGold() : base(CDefinePath.s_strPlunderSupportGoldURL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			PLUNDER_SUPPORT_GOLD pLUNDER_SUPPORT_GOLD = new PLUNDER_SUPPORT_GOLD();
			pLUNDER_SUPPORT_GOLD.SetData(data);
			NrTSingleton<NrTable_SupportGold_Manager>.Instance.Set_Value(pLUNDER_SUPPORT_GOLD);
		}
		return true;
	}
}
