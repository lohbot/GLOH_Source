using System;
using TsLibs;

public class NkTableMapInfo : NrTableBase
{
	public NkTableMapInfo() : base(CDefinePath.MAP_INFO_URL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<MAP_INFO>(dr);
	}
}
