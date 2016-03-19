using System;
using TsLibs;

public class EXTRACT_RATE : NrTableData
{
	public short Season;

	public short Grade;

	public int i32ExtrateRate;

	public int i32ExtrateHeartsRate;

	public override void SetData(TsDataReader.Row row)
	{
		int num = 0;
		int num2 = 0;
		row.GetColumn(num++, out this.Season);
		row.GetColumn(num++, out this.Grade);
		row.GetColumn(num++, out this.i32ExtrateRate);
		row.GetColumn(num++, out num2);
		row.GetColumn(num++, out num2);
		row.GetColumn(num++, out num2);
		row.GetColumn(num++, out num2);
		row.GetColumn(num++, out num2);
		row.GetColumn(num++, out num2);
		row.GetColumn(num++, out num2);
		row.GetColumn(num++, out num2);
		row.GetColumn(num++, out num2);
		row.GetColumn(num++, out this.i32ExtrateHeartsRate);
		row.GetColumn(num++, out num2);
		row.GetColumn(num++, out num2);
		row.GetColumn(num++, out num2);
		row.GetColumn(num++, out num2);
		row.GetColumn(num++, out num2);
		row.GetColumn(num++, out num2);
		row.GetColumn(num++, out num2);
		row.GetColumn(num++, out num2);
		row.GetColumn(num++, out num2);
	}
}
