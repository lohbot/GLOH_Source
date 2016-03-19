using System;
using TsLibs;

public class BABEL_GUILDBOSS : NrTableData
{
	public short m_nFloor;

	public int m_nScenario;

	public byte m_nBattlePos;

	public int m_nBossKind;

	public int m_nBossMaxHP;

	public string m_strTextExplain = string.Empty;

	public int m_nReward_BaseMoney;

	public int m_nReward_BaseMoney_Max;

	public int m_nReward_ClearMoney;

	public int m_nGuildPoint;

	public int m_nBaseReward_ItemUnique;

	public int m_nBaseReward_ItemNum;

	public int m_nRankReward_itemUnique;

	public int m_nRankReward_itemNum;

	public BABEL_GUILDBOSS()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_nFloor = 0;
		this.m_nScenario = 0;
		this.m_nBattlePos = 0;
		this.m_nBossKind = 0;
		this.m_nBossMaxHP = 0;
		this.m_strTextExplain = string.Empty;
		this.m_nReward_BaseMoney = 0;
		this.m_nReward_ClearMoney = 0;
		this.m_nGuildPoint = 0;
		this.m_nBaseReward_ItemUnique = 0;
		this.m_nBaseReward_ItemNum = 0;
		this.m_nRankReward_itemUnique = 0;
		this.m_nRankReward_itemNum = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		string empty = string.Empty;
		row.GetColumn(num++, out this.m_nFloor);
		row.GetColumn(num++, out this.m_nScenario);
		row.GetColumn(num++, out this.m_nBattlePos);
		row.GetColumn(num++, out this.m_nBossKind);
		row.GetColumn(num++, out empty);
		row.GetColumn(num++, out this.m_nBossMaxHP);
		row.GetColumn(num++, out this.m_strTextExplain);
		row.GetColumn(num++, out this.m_nReward_BaseMoney);
		row.GetColumn(num++, out this.m_nReward_BaseMoney_Max);
		row.GetColumn(num++, out this.m_nReward_ClearMoney);
		row.GetColumn(num++, out this.m_nGuildPoint);
		row.GetColumn(num++, out this.m_nBaseReward_ItemUnique);
		row.GetColumn(num++, out this.m_nBaseReward_ItemNum);
		row.GetColumn(num++, out this.m_nRankReward_itemUnique);
		row.GetColumn(num++, out this.m_nRankReward_itemNum);
	}
}
