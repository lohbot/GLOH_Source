using System;
using TsLibs;

public class NkTableMapUnit : NrTableBase
{
	public NkTableMapUnit() : base(CDefinePath.MAP_UNIT_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<MAP_UNIT>(dr);
	}
}
