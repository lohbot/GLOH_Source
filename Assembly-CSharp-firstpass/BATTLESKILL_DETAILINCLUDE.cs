using System;
using TsLibs;

public class BATTLESKILL_DETAILINCLUDE : NrTableData
{
	public int m_nSkillUnique;

	public int m_nSkillMinSkillLevel;

	public int m_nSkillNeedAngerlyPoint_I;

	public int m_nSkillSuccessRate_I;

	public int m_nSkillDurationTurn_I;

	public int m_nSkillDurationRate_I;

	public int m_nSkillAggroAdd_I;

	public int m_nSkillAggroDamagePer_I;

	public int[] m_nSkillDetalParamType_I = new int[10];

	public int[] m_nSkillDetalParamValue_I = new int[10];

	public BATTLESKILL_DETAILINCLUDE()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_nSkillUnique = 0;
		this.m_nSkillMinSkillLevel = 0;
		this.m_nSkillNeedAngerlyPoint_I = 0;
		this.m_nSkillSuccessRate_I = 0;
		this.m_nSkillDurationTurn_I = 0;
		this.m_nSkillDurationRate_I = 0;
		this.m_nSkillAggroAdd_I = 0;
		this.m_nSkillAggroDamagePer_I = 0;
		for (int i = 0; i < 10; i++)
		{
			this.m_nSkillDetalParamType_I[i] = 0;
			this.m_nSkillDetalParamValue_I[i] = 0;
		}
	}

	public int GetSkillDetalIncludeParamValue(int detailParamType)
	{
		for (int i = 0; i < 10; i++)
		{
			if (this.m_nSkillDetalParamType_I[i] == detailParamType)
			{
				return this.m_nSkillDetalParamValue_I[i];
			}
		}
		return 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		string empty = string.Empty;
		row.GetColumn(num++, out this.m_nSkillUnique);
		row.GetColumn(num++, out this.m_nSkillMinSkillLevel);
		row.GetColumn(num++, out this.m_nSkillNeedAngerlyPoint_I);
		row.GetColumn(num++, out this.m_nSkillSuccessRate_I);
		row.GetColumn(num++, out this.m_nSkillDurationTurn_I);
		row.GetColumn(num++, out this.m_nSkillDurationRate_I);
		row.GetColumn(num++, out this.m_nSkillAggroAdd_I);
		row.GetColumn(num++, out this.m_nSkillAggroDamagePer_I);
		row.GetColumn(num++, out empty);
		this.m_nSkillDetalParamType_I[0] = NrTSingleton<BATTLE_SKILL_PARSER>.Instance.GetParamType(empty);
		row.GetColumn(num++, out this.m_nSkillDetalParamValue_I[0]);
		row.GetColumn(num++, out empty);
		this.m_nSkillDetalParamType_I[1] = NrTSingleton<BATTLE_SKILL_PARSER>.Instance.GetParamType(empty);
		row.GetColumn(num++, out this.m_nSkillDetalParamValue_I[1]);
		row.GetColumn(num++, out empty);
		this.m_nSkillDetalParamType_I[2] = NrTSingleton<BATTLE_SKILL_PARSER>.Instance.GetParamType(empty);
		row.GetColumn(num++, out this.m_nSkillDetalParamValue_I[2]);
		row.GetColumn(num++, out empty);
		this.m_nSkillDetalParamType_I[3] = NrTSingleton<BATTLE_SKILL_PARSER>.Instance.GetParamType(empty);
		row.GetColumn(num++, out this.m_nSkillDetalParamValue_I[3]);
		row.GetColumn(num++, out empty);
		this.m_nSkillDetalParamType_I[4] = NrTSingleton<BATTLE_SKILL_PARSER>.Instance.GetParamType(empty);
		row.GetColumn(num++, out this.m_nSkillDetalParamValue_I[4]);
		row.GetColumn(num++, out empty);
		this.m_nSkillDetalParamType_I[5] = NrTSingleton<BATTLE_SKILL_PARSER>.Instance.GetParamType(empty);
		row.GetColumn(num++, out this.m_nSkillDetalParamValue_I[5]);
		row.GetColumn(num++, out empty);
		this.m_nSkillDetalParamType_I[6] = NrTSingleton<BATTLE_SKILL_PARSER>.Instance.GetParamType(empty);
		row.GetColumn(num++, out this.m_nSkillDetalParamValue_I[6]);
		row.GetColumn(num++, out empty);
		this.m_nSkillDetalParamType_I[7] = NrTSingleton<BATTLE_SKILL_PARSER>.Instance.GetParamType(empty);
		row.GetColumn(num++, out this.m_nSkillDetalParamValue_I[7]);
		row.GetColumn(num++, out empty);
		this.m_nSkillDetalParamType_I[8] = NrTSingleton<BATTLE_SKILL_PARSER>.Instance.GetParamType(empty);
		row.GetColumn(num++, out this.m_nSkillDetalParamValue_I[8]);
		row.GetColumn(num++, out empty);
		this.m_nSkillDetalParamType_I[9] = NrTSingleton<BATTLE_SKILL_PARSER>.Instance.GetParamType(empty);
		row.GetColumn(num++, out this.m_nSkillDetalParamValue_I[9]);
	}
}
