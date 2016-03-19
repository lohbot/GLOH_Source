using System;
using TsLibs;

public class BATTLE_GRID_DATA : NrTableData
{
	public int GRID_ID;

	public int POS_X;

	public int POS_Y;

	public BATTLE_GRID_DATA()
	{
		this.Init();
	}

	public void Init()
	{
		this.GRID_ID = 0;
		this.POS_X = 0;
		this.POS_Y = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.GRID_ID);
		row.GetColumn(num++, out this.POS_X);
		row.GetColumn(num++, out this.POS_Y);
	}
}
