using System;
using TsLibs;

public class NrTable_Item_Ticket : NrTableBase
{
	public NrTable_Item_Ticket() : base(CDefinePath.s_strItemTicketURL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<ITEM_TICKET>(dr);
	}
}
