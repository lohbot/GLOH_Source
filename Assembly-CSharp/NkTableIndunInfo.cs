using System;
using TsLibs;

public class NkTableIndunInfo : NrTableBase
{
	public NkTableIndunInfo() : base(CDefinePath.INDUN_INFO_URL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<INDUN_INFO>(dr);
	}
}
