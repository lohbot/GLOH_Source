using System;
using TsLibs;

public class ROAD_POINT : NrTableData
{
	public short ROADPOINT_IDX;

	public short MAP_IDX;

	public float POSX;

	public float POSY;

	public float POSZ;

	public short[] LINK_RPIDX;

	public ROAD_POINT() : base(NrTableData.eResourceType.MIN_eRT_NUM)
	{
		this.LINK_RPIDX = new short[4];
		this.Init();
	}

	public void Init()
	{
		this.ROADPOINT_IDX = 0;
		this.MAP_IDX = 0;
		this.POSX = 0f;
		this.POSY = 0f;
		this.POSZ = 0f;
		for (int i = 0; i < 4; i++)
		{
			this.LINK_RPIDX[i] = 0;
		}
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.ROADPOINT_IDX);
		row.GetColumn(num++, out this.MAP_IDX);
		row.GetColumn(num++, out this.POSX);
		row.GetColumn(num++, out this.POSY);
		row.GetColumn(num++, out this.POSZ);
		for (int i = 0; i < 4; i++)
		{
			row.GetColumn(num++, out this.LINK_RPIDX[i]);
		}
	}
}
