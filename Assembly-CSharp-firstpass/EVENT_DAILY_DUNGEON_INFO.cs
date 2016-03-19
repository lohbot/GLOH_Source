using System;
using TsLibs;

public class EVENT_DAILY_DUNGEON_INFO : NrTableData
{
	public sbyte i8DayOfWeek;

	public sbyte i8Difficulty;

	public int i32MonLevel;

	public sbyte i8TotalCount;

	public int[] i32Eco = new int[10];

	public int i32RewardItemUnique;

	public int i32RewardItemNum;

	public string szBGIMG = string.Empty;

	public int i32TextKey;

	public EVENT_DAILY_DUNGEON_INFO()
	{
		this.Init();
	}

	public void Init()
	{
		this.i8DayOfWeek = 0;
		this.i8Difficulty = 0;
		this.i32MonLevel = 0;
		this.i8TotalCount = 0;
		for (int i = 0; i < 10; i++)
		{
			this.i32Eco[i] = 0;
		}
		this.i32RewardItemUnique = 0;
		this.i32RewardItemNum = 0;
		this.szBGIMG = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.i8DayOfWeek);
		row.GetColumn(num++, out this.i8Difficulty);
		row.GetColumn(num++, out this.i32MonLevel);
		row.GetColumn(num++, out this.i8TotalCount);
		for (int i = 0; i < 10; i++)
		{
			row.GetColumn(num++, out this.i32Eco[i]);
		}
		row.GetColumn(num++, out this.i32RewardItemUnique);
		row.GetColumn(num++, out this.i32RewardItemNum);
		row.GetColumn(num++, out this.szBGIMG);
		row.GetColumn(num++, out this.i32TextKey);
	}
}
