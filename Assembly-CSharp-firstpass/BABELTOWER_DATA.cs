using System;
using TsLibs;

public class BABELTOWER_DATA : NrTableData
{
	public short m_nFloorType;

	public short m_nFloor;

	public short m_nSubFloor;

	public int m_nBossRate;

	public int m_nScenario;

	public short m_nMonLevel;

	public int m_nMaxChar;

	public string m_strTextExplain = string.Empty;

	public int m_nFirstReward_ItemUniq;

	public int m_nFirstReward_ItemNum;

	public int m_nReward_ItemUniq;

	public int m_nReward_ItemNum;

	public int m_nReward_Exp;

	public int m_nRankTurn;

	public int m_nSpecialRewardType;

	public int m_i32TreasureRewardUnique;

	public int m_i32TreasureRewardRank;

	public int m_i32TreasureRewardNum;

	public int m_i32ShowSpecialReward_DataUnique;

	public int m_i32ShowSpecialReward_DataPos;

	public short m_nWillSpend;

	public BABELTOWER_DATA()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_nFloorType = 0;
		this.m_nFloor = 0;
		this.m_nSubFloor = 0;
		this.m_nBossRate = 0;
		this.m_nScenario = 0;
		this.m_nMonLevel = 0;
		this.m_nMaxChar = 0;
		this.m_strTextExplain = string.Empty;
		this.m_nFirstReward_ItemUniq = 0;
		this.m_nFirstReward_ItemNum = 0;
		this.m_nReward_ItemUniq = 0;
		this.m_nReward_ItemNum = 0;
		this.m_nReward_Exp = 0;
		this.m_nRankTurn = 0;
		this.m_nSpecialRewardType = 0;
		this.m_i32TreasureRewardUnique = 0;
		this.m_i32TreasureRewardRank = 0;
		this.m_i32TreasureRewardNum = 0;
		this.m_i32ShowSpecialReward_DataUnique = 0;
		this.m_i32ShowSpecialReward_DataPos = 0;
		this.m_nWillSpend = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.m_nFloorType);
		row.GetColumn(num++, out this.m_nFloor);
		row.GetColumn(num++, out this.m_nSubFloor);
		row.GetColumn(num++, out this.m_nBossRate);
		row.GetColumn(num++, out this.m_nScenario);
		row.GetColumn(num++, out this.m_nMonLevel);
		row.GetColumn(num++, out this.m_nMaxChar);
		row.GetColumn(num++, out this.m_strTextExplain);
		row.GetColumn(num++, out this.m_nFirstReward_ItemUniq);
		row.GetColumn(num++, out this.m_nFirstReward_ItemNum);
		row.GetColumn(num++, out this.m_nReward_ItemUniq);
		row.GetColumn(num++, out this.m_nReward_ItemNum);
		row.GetColumn(num++, out this.m_nReward_Exp);
		row.GetColumn(num++, out this.m_nRankTurn);
		row.GetColumn(num++, out this.m_nSpecialRewardType);
		row.GetColumn(num++, out this.m_i32TreasureRewardUnique);
		row.GetColumn(num++, out this.m_i32TreasureRewardRank);
		row.GetColumn(num++, out this.m_i32TreasureRewardNum);
		row.GetColumn(num++, out this.m_i32ShowSpecialReward_DataUnique);
		row.GetColumn(num++, out this.m_i32ShowSpecialReward_DataPos);
		row.GetColumn(num++, out this.m_nWillSpend);
	}
}
