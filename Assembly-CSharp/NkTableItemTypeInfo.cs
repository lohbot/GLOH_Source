using System;
using TsLibs;

public class NkTableItemTypeInfo : NrTableBase
{
	public NkTableItemTypeInfo() : base(CDefinePath.ITEMTYPE_INFO_URL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<ITEMTYPE_INFO>(dr);
	}
}
