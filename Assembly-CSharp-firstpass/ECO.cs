using GAME;
using System;
using TsLibs;

public class ECO : NrTableData
{
	public int GroupUnique;

	public int MapIndex;

	public short nBattlePosUnique;

	public string[] szCharCode = new string[6];

	public int[] nBattlePos = new int[6];

	public POS3D kFixPos = new POS3D();

	public short nFixPosR;

	public short nFixPosA;

	public POS3D kRanPos = new POS3D();

	public short nRanPosR;

	public int nRangeChangeTime;

	public POS3D[] kMovePos = new POS3D[5];

	public int[] nMovePointTime = new int[5];

	public byte bDeletePatrolCharFlag;

	public byte bPatrolLoopFlag;

	public short i16MaxNum;

	public byte bGroupRegenFlag;

	public short i16GroupRegenNum;

	public int nLifeTime;

	public int nRegenTime;

	public short i16Ally;

	public int i32ScenarioCode;

	public int i32LinkGroupUnique;

	public string strATB = string.Empty;

	public long i64ATB;

	public byte i8RankNum;

	public ECO()
	{
		for (int i = 0; i < 5; i++)
		{
			this.kMovePos[i] = new POS3D();
		}
		base.SetTypeIndex(NrTableData.eResourceType.eRT_ECO);
		this.Init();
	}

	public void Init()
	{
		this.GroupUnique = 0;
		this.MapIndex = 0;
		for (int i = 0; i < 6; i++)
		{
			this.szCharCode[i] = string.Empty;
			this.nBattlePos[i] = -1;
		}
		this.kFixPos.x = 0f;
		this.kFixPos.y = 0f;
		this.kFixPos.z = 0f;
		this.nFixPosR = 0;
		this.nFixPosA = 0;
		this.kRanPos.x = 0f;
		this.kRanPos.y = 0f;
		this.nRanPosR = 0;
		this.nRangeChangeTime = 0;
		for (int j = 0; j < 5; j++)
		{
			this.kMovePos[j].x = 0f;
			this.kMovePos[j].y = 0f;
			this.kMovePos[j].z = 0f;
			this.nMovePointTime[j] = 0;
		}
		this.bDeletePatrolCharFlag = 0;
		this.bPatrolLoopFlag = 0;
		this.i16MaxNum = 0;
		this.bGroupRegenFlag = 0;
		this.i16GroupRegenNum = 0;
		this.nLifeTime = 0;
		this.nRegenTime = 0;
		this.i16Ally = 0;
		this.i32ScenarioCode = 0;
		this.i32LinkGroupUnique = 0;
		this.i64ATB = 0L;
		this.i8RankNum = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.GroupUnique);
		row.GetColumn(num++, out this.MapIndex);
		row.GetColumn(num++, out this.nBattlePosUnique);
		row.GetColumn(num++, out this.szCharCode[0]);
		row.GetColumn(num++, out this.nBattlePos[0]);
		row.GetColumn(num++, out this.szCharCode[1]);
		row.GetColumn(num++, out this.nBattlePos[1]);
		row.GetColumn(num++, out this.szCharCode[2]);
		row.GetColumn(num++, out this.nBattlePos[2]);
		row.GetColumn(num++, out this.szCharCode[3]);
		row.GetColumn(num++, out this.nBattlePos[3]);
		row.GetColumn(num++, out this.szCharCode[4]);
		row.GetColumn(num++, out this.nBattlePos[4]);
		row.GetColumn(num++, out this.szCharCode[5]);
		row.GetColumn(num++, out this.nBattlePos[5]);
		row.GetColumn(num++, out this.kFixPos.x);
		row.GetColumn(num++, out this.kFixPos.y);
		row.GetColumn(num++, out this.kFixPos.z);
		row.GetColumn(num++, out this.nFixPosR);
		row.GetColumn(num++, out this.nFixPosA);
		row.GetColumn(num++, out this.kRanPos.x);
		row.GetColumn(num++, out this.kRanPos.y);
		row.GetColumn(num++, out this.kRanPos.z);
		row.GetColumn(num++, out this.nRanPosR);
		row.GetColumn(num++, out this.nRangeChangeTime);
		row.GetColumn(num++, out this.kMovePos[0].x);
		row.GetColumn(num++, out this.kMovePos[0].y);
		row.GetColumn(num++, out this.kMovePos[0].z);
		row.GetColumn(num++, out this.nMovePointTime[0]);
		row.GetColumn(num++, out this.kMovePos[1].x);
		row.GetColumn(num++, out this.kMovePos[1].y);
		row.GetColumn(num++, out this.kMovePos[1].z);
		row.GetColumn(num++, out this.nMovePointTime[1]);
		row.GetColumn(num++, out this.kMovePos[2].x);
		row.GetColumn(num++, out this.kMovePos[2].y);
		row.GetColumn(num++, out this.kMovePos[2].z);
		row.GetColumn(num++, out this.nMovePointTime[2]);
		row.GetColumn(num++, out this.kMovePos[3].x);
		row.GetColumn(num++, out this.kMovePos[3].y);
		row.GetColumn(num++, out this.kMovePos[3].z);
		row.GetColumn(num++, out this.nMovePointTime[3]);
		row.GetColumn(num++, out this.kMovePos[4].x);
		row.GetColumn(num++, out this.kMovePos[4].y);
		row.GetColumn(num++, out this.kMovePos[4].z);
		row.GetColumn(num++, out this.nMovePointTime[4]);
		row.GetColumn(num++, out this.bDeletePatrolCharFlag);
		row.GetColumn(num++, out this.bPatrolLoopFlag);
		row.GetColumn(num++, out this.i16MaxNum);
		row.GetColumn(num++, out this.bGroupRegenFlag);
		row.GetColumn(num++, out this.i16GroupRegenNum);
		row.GetColumn(num++, out this.nLifeTime);
		row.GetColumn(num++, out this.nRegenTime);
		row.GetColumn(num++, out this.i16Ally);
		row.GetColumn(num++, out this.i32ScenarioCode);
		row.GetColumn(num++, out this.i32LinkGroupUnique);
		row.GetColumn(num++, out this.strATB);
		row.GetColumn(num++, out this.i8RankNum);
	}

	public bool IsEcoATB(long nFlag)
	{
		return (this.i64ATB & nFlag) != 0L;
	}
}
