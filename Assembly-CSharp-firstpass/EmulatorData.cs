using System;
using TsLibs;

public class EmulatorData : NrTableData
{
	public string package = string.Empty;

	public int type;

	public EmulatorData()
	{
		this.Init();
	}

	public void Init()
	{
		this.package = string.Empty;
		this.type = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.package);
		row.GetColumn(num++, out this.type);
	}
}
