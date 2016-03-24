using System;
using TsLibs;

public class NkTableLocalMapInfo : NrTableBase
{
	public NkTableLocalMapInfo() : base(CDefinePath.LOCALMAP_INFO_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<LOCALMAP_INFO>(dr);
	}
}
