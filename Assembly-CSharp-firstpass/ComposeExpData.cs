using System;
using TsLibs;

public class ComposeExpData : NrTableData
{
	public byte SolLevel;

	public long[] GradeExp = new long[15];

	public ComposeExpData()
	{
		this.Init();
	}

	public void Init()
	{
		this.SolLevel = 0;
		for (int i = 0; i < 15; i++)
		{
			this.GradeExp[i] = 0L;
		}
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.SolLevel);
		row.GetColumn(num++, out this.GradeExp[0]);
		row.GetColumn(num++, out this.GradeExp[1]);
		row.GetColumn(num++, out this.GradeExp[2]);
		row.GetColumn(num++, out this.GradeExp[3]);
		row.GetColumn(num++, out this.GradeExp[4]);
		row.GetColumn(num++, out this.GradeExp[5]);
	}
}
