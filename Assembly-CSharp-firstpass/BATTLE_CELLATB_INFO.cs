using System;
using TsLibs;

public class BATTLE_CELLATB_INFO : NrTableData
{
	public int MAPID;

	public int SIZEX;

	public int SIZEY;

	public string TILE = string.Empty;

	public BATTLE_CELLATB_INFO()
	{
		this.Init();
	}

	public void Init()
	{
		this.MAPID = 0;
		this.SIZEX = 0;
		this.SIZEY = 0;
		this.TILE = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.MAPID);
		row.GetColumn(num++, out this.SIZEX);
		row.GetColumn(num++, out this.SIZEY);
		row.GetColumn(num++, out this.TILE);
	}
}
