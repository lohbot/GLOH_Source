using System;
using TsLibs;

public class Ticket_Info : NrTableData
{
	public int Grade;

	public int[] GainRate = new int[23];

	public Ticket_Info() : base(NrTableData.eResourceType.eRT_SOLDIER_TICKETINFO)
	{
		this.Init();
	}

	public void Init()
	{
		this.Grade = 0;
		for (int i = 0; i < 23; i++)
		{
			this.GainRate[i] = 0;
		}
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.Grade);
		row.GetColumn(num++, out this.GainRate[0]);
		row.GetColumn(num++, out this.GainRate[1]);
		row.GetColumn(num++, out this.GainRate[2]);
		row.GetColumn(num++, out this.GainRate[3]);
		row.GetColumn(num++, out this.GainRate[4]);
		row.GetColumn(num++, out this.GainRate[5]);
	}
}
