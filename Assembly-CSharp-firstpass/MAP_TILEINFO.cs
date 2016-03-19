using System;
using TsLibs;

public class MAP_TILEINFO : NrTableData
{
	public int MAP_IDX;

	public short HEIGHT;

	public short WIDTH;

	public byte TILESIZE;

	public string TILE = string.Empty;

	public MAP_TILEINFO()
	{
		this.Init();
	}

	public void Init()
	{
		this.MAP_IDX = 0;
		this.HEIGHT = 0;
		this.WIDTH = 0;
		this.TILESIZE = 0;
		this.TILE = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.MAP_IDX);
		row.GetColumn(num++, out this.HEIGHT);
		row.GetColumn(num++, out this.WIDTH);
		row.GetColumn(num++, out this.TILESIZE);
		row.GetColumn(num++, out this.TILE);
	}

	public override void SetData(TsDataReader.Row row, Type objtype)
	{
		int num = 0;
		row.GetColumn(num++, out this.MAP_IDX);
		row.GetColumn(num++, out this.HEIGHT);
		row.GetColumn(num++, out this.WIDTH);
		row.GetColumn(num++, out this.TILESIZE);
	}
}
