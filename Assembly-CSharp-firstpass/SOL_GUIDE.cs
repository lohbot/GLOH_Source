using System;
using TsLibs;

public class SOL_GUIDE : NrTableData
{
	public byte m_bSeason;

	public string m_strCharCode = string.Empty;

	public int m_i32CharKind;

	public int m_iSolGrade;

	public byte m_bFlagSet;

	public byte m_bFlagSetCount;

	public int m_i32SkillUnique;

	public int m_i32SkillText;

	public byte m_i8Alchemy;

	public byte m_i8Legend;

	public short m_i16LegendSort;

	public SOL_GUIDE() : base(NrTableData.eResourceType.eRT_CHARSOL_GUIDE)
	{
		this.Init();
	}

	public void Init()
	{
		this.m_bSeason = 0;
		this.m_strCharCode = string.Empty;
		this.m_i32CharKind = 0;
		this.m_iSolGrade = 0;
		this.m_bFlagSet = 0;
		this.m_bFlagSetCount = 0;
		this.m_i32SkillUnique = 0;
		this.m_i32SkillText = 0;
		this.m_i8Alchemy = 1;
		this.m_i8Legend = 1;
		this.m_i16LegendSort = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.m_bSeason);
		row.GetColumn(num++, out this.m_strCharCode);
		row.GetColumn(num++, out this.m_iSolGrade);
		row.GetColumn(num++, out this.m_bFlagSet);
		row.GetColumn(num++, out this.m_bFlagSetCount);
		row.GetColumn(num++, out this.m_i32SkillUnique);
		row.GetColumn(num++, out this.m_i32SkillText);
		row.GetColumn(num++, out this.m_i8Alchemy);
		row.GetColumn(num++, out this.m_i8Legend);
	}
}
