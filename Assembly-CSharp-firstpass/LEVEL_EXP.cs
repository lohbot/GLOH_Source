using System;
using TsLibs;

public class LEVEL_EXP : NrTableData
{
	public string EXP_TYPE;

	public short LEVEL;

	public long EXP;

	public LEVEL_EXP()
	{
		this.Init();
	}

	public void Init()
	{
		this.EXP_TYPE = string.Empty;
		this.LEVEL = 0;
		this.EXP = 0L;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.EXP_TYPE);
		row.GetColumn(num++, out this.LEVEL);
		row.GetColumn(num++, out this.EXP);
	}
}
