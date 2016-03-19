using System;
using TsLibs;

public class NkTableSoldierTicketInfo : NrTableBase
{
	public NkTableSoldierTicketInfo() : base(CDefinePath.SOLDIER_TICKETINFO_URL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<Ticket_Info>(dr);
	}
}
