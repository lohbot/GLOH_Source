using System;
using TsLibs;

public class NEWEXPLORATION_DATA
{
	public short i16Index;

	public sbyte bFloorType;

	public sbyte bFloor;

	public sbyte bSubFloor;

	public int i32Scenario;

	public short i16MonLevel;

	public int i32BossCharKind;

	public int i32BossHp;

	public int i32RewardExp;

	public int i32RewardCount;

	public NEWEXPLORATION_DATA()
	{
		this.Init();
	}

	public void Init()
	{
		this.i16Index = 0;
		this.bFloorType = 0;
		this.bFloor = 0;
		this.bSubFloor = 0;
		this.i32Scenario = 0;
		this.i16MonLevel = 0;
		this.i32BossCharKind = 0;
		this.i32BossHp = 0;
		this.i32RewardCount = 0;
	}

	public string SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		string empty = string.Empty;
		byte b = 0;
		row.GetColumn(num++, out this.bFloorType);
		row.GetColumn(num++, out this.bFloor);
		row.GetColumn(num++, out this.bSubFloor);
		row.GetColumn(num++, out this.i32Scenario);
		row.GetColumn(num++, out this.i16MonLevel);
		row.GetColumn(num++, out empty);
		row.GetColumn(num++, out b);
		row.GetColumn(num++, out this.i32BossHp);
		string empty2 = string.Empty;
		for (int i = 0; i < 12; i++)
		{
			row.GetColumn(num++, out empty2);
			row.GetColumn(num++, out b);
		}
		row.GetColumn(num++, out this.i32RewardExp);
		row.GetColumn(num++, out num2);
		row.GetColumn(num++, out num3);
		row.GetColumn(num++, out this.i32RewardCount);
		this.i16Index = (short)this.bSubFloor;
		this.i16Index += (short)((int)this.bFloor * 100);
		return empty;
	}
}
