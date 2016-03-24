using System;
using TsLibs;

public class NrTableTimeShopRefresh : NrTableBase
{
	public NrTableTimeShopRefresh() : base(CDefinePath.TIMESHOPREFRESH_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		NrTSingleton<NrTableTimeShopManager>.Instance.Clear_RefreshDataValue();
		foreach (TsDataReader.Row data in dr)
		{
			TIMESHOP_REFRESHDATA tIMESHOP_REFRESHDATA = new TIMESHOP_REFRESHDATA();
			tIMESHOP_REFRESHDATA.SetData(data);
			NrTSingleton<NrTableTimeShopManager>.Instance.Set_RefreshDataValue(tIMESHOP_REFRESHDATA);
		}
		return true;
	}
}
