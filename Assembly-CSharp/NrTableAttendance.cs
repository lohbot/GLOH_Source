using System;
using TsLibs;

public class NrTableAttendance : NrTableBase
{
	public NrTableAttendance() : base(CDefinePath.ATTENDANCE)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			ATTENDANCE aTTENDANCE = new ATTENDANCE();
			aTTENDANCE.SetData(data);
			NrTSingleton<NrAttendance_Manager>.Instance.AddData(aTTENDANCE);
		}
		return true;
	}
}
