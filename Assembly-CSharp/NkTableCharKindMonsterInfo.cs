using System;
using TsLibs;

public class NkTableCharKindMonsterInfo : NrTableBase
{
	public NkTableCharKindMonsterInfo() : base(CDefinePath.CHARKIND_MONSTERINFO_URL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<CHARKIND_MONSTERINFO>(dr);
	}
}
