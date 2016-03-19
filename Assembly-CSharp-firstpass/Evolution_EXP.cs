using System;
using TsLibs;

public class Evolution_EXP : NrTableData
{
	public int Grade;

	public long NeedEXP;

	public Evolution_EXP() : base(NrTableData.eResourceType.eRT_SOLDIER_EVOLUTIONEXP)
	{
		this.Init();
	}

	public void Init()
	{
		this.Grade = 0;
		this.NeedEXP = 0L;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.Grade);
		row.GetColumn(num++, out this.NeedEXP);
	}
}
