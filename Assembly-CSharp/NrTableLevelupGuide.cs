using System;
using TsLibs;

public class NrTableLevelupGuide : NrTableBase
{
	public NrTableLevelupGuide() : base(CDefinePath.LEVELUP_GUIDE_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<LEVELUPGUIDE_INFO>(dr);
	}
}
