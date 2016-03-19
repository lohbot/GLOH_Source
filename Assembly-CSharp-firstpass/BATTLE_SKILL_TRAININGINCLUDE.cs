using System;
using TsLibs;

public class BATTLE_SKILL_TRAININGINCLUDE : NrTableData
{
	public int m_nSkillUnique;

	public int m_nSkillMinSkillLevel;

	public int m_nSkillNeedGold_I;

	public BATTLE_SKILL_TRAININGINCLUDE()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_nSkillUnique = 0;
		this.m_nSkillMinSkillLevel = 0;
		this.m_nSkillNeedGold_I = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.m_nSkillUnique);
		row.GetColumn(num++, out this.m_nSkillMinSkillLevel);
		row.GetColumn(num++, out this.m_nSkillNeedGold_I);
	}
}
