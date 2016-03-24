using System;
using TsLibs;

public class NrTable_AutoSell : NrTableBase
{
	public NrTable_AutoSell() : base(CDefinePath.AUTOSELL_INFO_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<AutoSell_info>(dr);
	}
}
