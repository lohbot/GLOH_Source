using System;
using TsLibs;

public class NkTableWorldMapInfo : NrTableBase
{
	public NkTableWorldMapInfo() : base(CDefinePath.WORLDMAP_INFO_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<WORLDMAP_INFO>(dr);
	}
}
