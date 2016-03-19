using System;
using TsLibs;

public class NkTableGateInfo : NrTableBase
{
	public NkTableGateInfo() : base(CDefinePath.GATE_INFO_URL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<GATE_INFO>(dr);
	}
}
