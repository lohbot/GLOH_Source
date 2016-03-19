using System;
using TsLibs;

public class MAP_UNIT : NrTableData
{
	public int MAP_UNIQUE;

	public int MAP_IDX;

	public string MAP_NAME = string.Empty;

	public MAP_UNIT() : base(NrTableData.eResourceType.eRT_MAP_UNIT)
	{
		this.Init();
	}

	public void Init()
	{
		this.MAP_UNIQUE = 0;
		this.MAP_IDX = 0;
		this.MAP_NAME = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.MAP_UNIQUE);
		row.GetColumn(num++, out this.MAP_IDX);
		row.GetColumn(num++, out this.MAP_NAME);
	}
}
