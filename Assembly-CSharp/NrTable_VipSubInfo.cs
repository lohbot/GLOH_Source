using System;
using TsLibs;

public class NrTable_VipSubInfo : NrTableBase
{
	public NrTable_VipSubInfo() : base(CDefinePath.VIPSUBINFO_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			VipSubInfo vipSubInfo = new VipSubInfo();
			vipSubInfo.SetData(data);
			NrTSingleton<NrVipSubInfoManager>.Instance.Set_Value(vipSubInfo);
		}
		return true;
	}
}
