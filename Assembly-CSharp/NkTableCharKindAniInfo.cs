using System;
using TsLibs;

public class NkTableCharKindAniInfo : NrTableBase
{
	public NkTableCharKindAniInfo() : base(CDefinePath.CHARKIND_ANIMATIONINFO_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<CHARKIND_ANIINFO>(dr);
	}
}
