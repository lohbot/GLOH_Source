using System;
using TsLibs;

public class NrTableTimeShop : NrTableBase
{
	public NrTableTimeShop() : base(CDefinePath.TIMESHOP_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		NrTSingleton<NrTableTimeShopManager>.Instance.Clear_DataValue();
		foreach (TsDataReader.Row data in dr)
		{
			TIMESHOP_DATA tIMESHOP_DATA = new TIMESHOP_DATA();
			tIMESHOP_DATA.SetData(data);
			NrTSingleton<NrTableTimeShopManager>.Instance.Set_DataValue(tIMESHOP_DATA);
		}
		return true;
	}
}
