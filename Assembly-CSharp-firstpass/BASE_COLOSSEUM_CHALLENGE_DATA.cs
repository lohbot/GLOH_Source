using System;
using TsLibs;

public class BASE_COLOSSEUM_CHALLENGE_DATA : NrTableData
{
	public int m_i32Index;

	public int m_i32EcoIndex;

	public int m_i32FirstReward_ItemUnique;

	public int m_i32FirstReqard_ItemNum;

	public int m_i32Buff;

	public int m_i32InterfaceKey;

	public int m_nSummary;

	public int m_i32ScenarioUnique;

	public BASE_COLOSSEUM_CHALLENGE_DATA()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_i32Index = 0;
		this.m_i32EcoIndex = 0;
		this.m_i32FirstReward_ItemUnique = 0;
		this.m_i32FirstReqard_ItemNum = 0;
		this.m_i32Buff = 0;
		this.m_i32InterfaceKey = 0;
		this.m_nSummary = 0;
		this.m_i32ScenarioUnique = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.m_i32Index);
		row.GetColumn(num++, out this.m_i32EcoIndex);
		row.GetColumn(num++, out this.m_i32FirstReward_ItemUnique);
		row.GetColumn(num++, out this.m_i32FirstReqard_ItemNum);
		row.GetColumn(num++, out this.m_i32Buff);
		row.GetColumn(num++, out this.m_i32InterfaceKey);
		row.GetColumn(num++, out this.m_nSummary);
		row.GetColumn(num++, out this.m_i32ScenarioUnique);
	}
}
