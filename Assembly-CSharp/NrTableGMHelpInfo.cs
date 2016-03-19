using System;
using TsLibs;

public class NrTableGMHelpInfo : NrTableBase
{
	public NrTableGMHelpInfo() : base(CDefinePath.GMHElpInfo_URL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<GMHELP_INFO>(dr);
	}
}
