using System;
using TsLibs;

public class BATTLE_MAP_CELL_INFO : NrTableData
{
	public short BATTLE_MAP_ID;

	public short CELLCOUNT_X;

	public short CELLCOUNT_Y;

	public string TILE = string.Empty;

	public bool bLoadComplete;

	public void Init()
	{
		this.BATTLE_MAP_ID = 0;
		this.CELLCOUNT_X = 0;
		this.CELLCOUNT_Y = 0;
		this.TILE = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.BATTLE_MAP_ID);
		row.GetColumn(num++, out this.CELLCOUNT_X);
		row.GetColumn(num++, out this.CELLCOUNT_Y);
		row.GetColumn(num++, out this.TILE);
		this.bLoadComplete = true;
	}
}
