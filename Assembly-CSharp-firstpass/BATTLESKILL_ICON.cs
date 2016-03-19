using System;
using TsLibs;

public class BATTLESKILL_ICON : NrTableData
{
	public string m_nSkillIconCode = string.Empty;

	public string m_strSkillIconFile = string.Empty;

	public int m_nSkillIconIndex;

	public string m_nSkillIconDESC = string.Empty;

	public BATTLESKILL_ICON()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_nSkillIconCode = string.Empty;
		this.m_strSkillIconFile = string.Empty;
		this.m_nSkillIconIndex = 0;
		this.m_nSkillIconDESC = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.m_nSkillIconCode);
		row.GetColumn(num++, out this.m_strSkillIconFile);
		row.GetColumn(num++, out this.m_nSkillIconIndex);
		row.GetColumn(num++, out this.m_nSkillIconDESC);
	}
}
