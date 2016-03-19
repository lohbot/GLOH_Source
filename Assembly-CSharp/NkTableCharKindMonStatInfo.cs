using System;
using TsLibs;

public class NkTableCharKindMonStatInfo : NrTableBase
{
	public NkTableCharKindMonStatInfo() : base(CDefinePath.CHARKIND_MONSTATINFO_URL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<CHARKIND_MONSTATINFO>(dr);
	}
}
