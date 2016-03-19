using System;
using TsLibs;

public class NrTable_CharSpend : NrTableBase
{
	public NrTable_CharSpend() : base(CDefinePath.CHARCHANGE_URL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<charSpend>(dr);
	}
}
