using System;
using TsLibs;

public class NkTableCharKindAttackInfo : NrTableBase
{
	public NkTableCharKindAttackInfo() : base(CDefinePath.CHARKIND_ATTACKINFO_URL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<CHARKIND_ATTACKINFO>(dr);
	}
}
