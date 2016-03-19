using System;
using TsLibs;

public class BATTLE_GRID_CELL : NrTableData
{
	public int GRID_ID;

	public int CELL;

	public int POS_X;

	public int POS_Y;

	public int PREV_CELL;

	public BATTLE_GRID_CELL()
	{
		this.Init();
	}

	public void Init()
	{
		this.GRID_ID = 0;
		this.CELL = 0;
		this.POS_X = 0;
		this.POS_Y = 0;
		this.PREV_CELL = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.GRID_ID);
		row.GetColumn(num++, out this.CELL);
		row.GetColumn(num++, out this.POS_X);
		row.GetColumn(num++, out this.POS_Y);
		row.GetColumn(num++, out this.PREV_CELL);
	}
}
