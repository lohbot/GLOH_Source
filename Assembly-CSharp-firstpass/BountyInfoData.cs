using System;
using TsLibs;

public class BountyInfoData : NrTableData
{
	public short i16Unique;

	public short i16Week;

	public short i16Hour;

	public short i16Page;

	public short i16Episode;

	public int i32NPCCharKind;

	public int i32ScenarioUnique;

	public short i16MonsterLevel;

	public short i16MaxChar;

	public short i16EcoIndex;

	public int i32FirstRewardUnique;

	public int i32FirstRewardNum;

	public int i32RepeatRewardUnique;

	public int i32RepeatRewardNum;

	public long i64RewardExp;

	public short i16RankBaseTurn;

	public short i16SpecialRewardType;

	public int i32MapIndex;

	public float[] fFixPosX = new float[5];

	public float[] fFixPosY = new float[5];

	public float[] fFixPosZ = new float[5];

	public int[] iDirection = new int[5];

	public int i32WeekTitleKey;

	public string strWeekBG = string.Empty;

	public string strMonBG = string.Empty;

	public string strNPCCharCode = string.Empty;

	public BountyInfoData()
	{
		this.Init();
	}

	public void Init()
	{
		this.i16Unique = 0;
		this.i16Week = 0;
		this.i16Hour = 0;
		this.i16Page = 0;
		this.i16Episode = 0;
		this.i32NPCCharKind = 0;
		this.i32ScenarioUnique = 0;
		this.i16MonsterLevel = 0;
		this.i16MaxChar = 0;
		this.i16EcoIndex = 0;
		this.i32FirstRewardUnique = 0;
		this.i32FirstRewardNum = 0;
		this.i32RepeatRewardUnique = 0;
		this.i32RepeatRewardNum = 0;
		this.i64RewardExp = 0L;
		this.i16RankBaseTurn = 0;
		this.i16SpecialRewardType = 0;
		this.i32MapIndex = 0;
		for (int i = 0; i < 5; i++)
		{
			this.fFixPosX[i] = 0f;
			this.fFixPosY[i] = 0f;
			this.fFixPosZ[i] = 0f;
			this.iDirection[i] = 0;
		}
		this.i32WeekTitleKey = 0;
		this.strWeekBG = string.Empty;
		this.strMonBG = string.Empty;
		this.strNPCCharCode = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.i16Unique);
		row.GetColumn(num++, out this.i16Week);
		row.GetColumn(num++, out this.i16Hour);
		row.GetColumn(num++, out this.i16Page);
		row.GetColumn(num++, out this.i16Episode);
		row.GetColumn(num++, out this.strNPCCharCode);
		row.GetColumn(num++, out this.i32ScenarioUnique);
		row.GetColumn(num++, out this.i16MonsterLevel);
		row.GetColumn(num++, out this.i16MaxChar);
		row.GetColumn(num++, out this.i16EcoIndex);
		row.GetColumn(num++, out this.i32FirstRewardUnique);
		row.GetColumn(num++, out this.i32FirstRewardNum);
		row.GetColumn(num++, out this.i32RepeatRewardUnique);
		row.GetColumn(num++, out this.i32RepeatRewardNum);
		row.GetColumn(num++, out this.i64RewardExp);
		row.GetColumn(num++, out this.i16RankBaseTurn);
		row.GetColumn(num++, out this.i16SpecialRewardType);
		row.GetColumn(num++, out this.i32MapIndex);
		for (int i = 0; i < 5; i++)
		{
			row.GetColumn(num++, out this.fFixPosX[i]);
			row.GetColumn(num++, out this.fFixPosY[i]);
			row.GetColumn(num++, out this.fFixPosZ[i]);
			row.GetColumn(num++, out this.iDirection[i]);
		}
		row.GetColumn(num++, out this.i32WeekTitleKey);
		row.GetColumn(num++, out this.strWeekBG);
		row.GetColumn(num++, out this.strMonBG);
	}
}
