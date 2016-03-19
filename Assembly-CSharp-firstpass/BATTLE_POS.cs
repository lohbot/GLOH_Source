using System;
using TsLibs;

public class BATTLE_POS : NrTableData
{
	public int GRID_ID;

	public int CELL;

	public int POS_WIDTH;

	public int POS_HEIGHT;

	public float POS_X;

	public float POS_Y;

	public BATTLE_POS()
	{
		this.Init();
	}

	public void Init()
	{
		this.GRID_ID = 0;
		this.CELL = 0;
		this.POS_X = 0f;
		this.POS_Y = 0f;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.GRID_ID);
		row.GetColumn(num++, out this.CELL);
		row.GetColumn(num++, out this.POS_WIDTH);
		row.GetColumn(num++, out this.POS_HEIGHT);
		row.GetColumn(num++, out this.POS_X);
		row.GetColumn(num++, out this.POS_Y);
	}
}
