using System;
using TsLibs;

public class NkTableCharKindStatInfo : NrTableBase
{
	public NkTableCharKindStatInfo() : base(CDefinePath.CHARKIND_STATINFO_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<CHARKIND_STATINFO>(dr);
	}
}
