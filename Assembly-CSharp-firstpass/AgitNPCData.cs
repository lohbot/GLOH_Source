using System;
using TsLibs;

public class AgitNPCData : NrTableData
{
	public byte ui8NPCType;

	public string strCharCode = string.Empty;

	public int[] i32LevelRate = new int[10];

	public float fPosX;

	public float fPosY;

	public float fDirection;

	public AgitNPCData() : base(NrTableData.eResourceType.eRT_AGIT_NPC)
	{
		this.Init();
	}

	public void Init()
	{
		this.ui8NPCType = 0;
		this.strCharCode = string.Empty;
		for (int i = 0; i < this.i32LevelRate.Length; i++)
		{
			this.i32LevelRate[i] = 0;
		}
		this.fPosX = 0f;
		this.fPosY = 0f;
		this.fDirection = 0f;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		row.GetColumn(0, out this.ui8NPCType);
		row.GetColumn(1, out this.strCharCode);
		row.GetColumn(2, out this.i32LevelRate[0]);
		row.GetColumn(3, out this.i32LevelRate[1]);
		row.GetColumn(4, out this.i32LevelRate[2]);
		row.GetColumn(5, out this.i32LevelRate[3]);
		row.GetColumn(6, out this.i32LevelRate[4]);
		row.GetColumn(7, out this.i32LevelRate[5]);
		row.GetColumn(8, out this.i32LevelRate[6]);
		row.GetColumn(9, out this.i32LevelRate[7]);
		row.GetColumn(10, out this.i32LevelRate[8]);
		row.GetColumn(11, out this.i32LevelRate[9]);
		row.GetColumn(12, out this.fPosX);
		row.GetColumn(13, out this.fPosY);
		row.GetColumn(14, out this.fDirection);
	}
}
