using System;
using TsLibs;

public class NkTableWeaponTypeInfo : NrTableBase
{
	public NkTableWeaponTypeInfo() : base(CDefinePath.WEAPONTYPE_INFO_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<WEAPONTYPE_INFO>(dr);
	}
}
