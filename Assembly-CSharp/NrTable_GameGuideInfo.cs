using System;
using TsLibs;

public class NrTable_GameGuideInfo : NrTableBase
{
	public NrTable_GameGuideInfo() : base(CDefinePath.GAMEGUIDE_INFO_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<TableData_GameGuideInfo>(dr);
	}
}
