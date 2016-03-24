using System;
using TsLibs;

public class NrTable_Group_Sol_Ticket : NrTableBase
{
	public NrTable_Group_Sol_Ticket() : base(CDefinePath.s_strItemGroupSolTicketURL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<GROUP_SOL_TICKET>(dr);
	}
}
