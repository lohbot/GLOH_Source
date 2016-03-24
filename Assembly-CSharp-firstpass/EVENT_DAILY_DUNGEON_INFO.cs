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

	public int i32ResetSoulGem;

	public string szOpen = string.Empty;

	public string szClose = string.Empty;

	public int i32MapIDX;

	public float fGridX;

	public float fGridY;

	public float fGridZ;

	public string szMonIMG = string.Empty;

	public int i32ExplainText;

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
		this.i32ResetSoulGem = 0;
		this.szOpen = string.Empty;
		this.szClose = string.Empty;
		this.i32MapIDX = 0;
		this.fGridX = 0f;
		this.fGridY = 0f;
		this.fGridZ = 0f;
		this.szMonIMG = string.Empty;
		this.i32ExplainText = 0;
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
		row.GetColumn(num++, out this.i32ResetSoulGem);
		row.GetColumn(num++, out this.szOpen);
		row.GetColumn(num++, out this.szClose);
		row.GetColumn(num++, out this.i32MapIDX);
		row.GetColumn(num++, out this.fGridX);
		row.GetColumn(num++, out this.fGridY);
		row.GetColumn(num++, out this.fGridZ);
		row.GetColumn(num++, out this.szMonIMG);
		row.GetColumn(num++, out this.i32ExplainText);
	}
}
