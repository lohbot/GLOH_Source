using System;
using TsLibs;

public class NrTableCharKindInfo : NrTableBase
{
	public NrTableCharKindInfo() : base(CDefinePath.CHARKIND_INFO_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<CHARKIND_INFO>(dr);
	}
}
