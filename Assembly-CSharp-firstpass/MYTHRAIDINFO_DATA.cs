using System;
using TsLibs;

public class MYTHRAIDINFO_DATA : NrTableData
{
	public int nRaidSeason;

	public short nRaidType;

	public int i32Scenario;

	public int i32MaxChar;

	public int i32TextExplain;

	public int nMainBossCharKind;

	public long i64BossHP;

	public short m_i16BatchMapIndex;

	public float m_fBatchMapGrideX;

	public float m_fBatchMapGrideY;

	public float m_fBatchMapGrideZ;

	private string nMainBossCharCode = string.Empty;

	public MYTHRAIDINFO_DATA() : base(NrTableData.eResourceType.eRT_MYTHRAIDINFO)
	{
		this.nRaidSeason = 0;
		this.nRaidType = 0;
		this.i32Scenario = 0;
		this.i32MaxChar = 0;
		this.i32TextExplain = 0;
		this.nMainBossCharKind = 0;
		this.i64BossHP = 0L;
		this.m_i16BatchMapIndex = 0;
		this.m_fBatchMapGrideX = 0f;
		this.m_fBatchMapGrideY = 0f;
		this.m_fBatchMapGrideZ = 0f;
	}

	public override void SetData(TsDataReader.Row row)
	{
		int num = 0;
		row.GetColumn(num++, out this.nRaidSeason);
		row.GetColumn(num++, out this.nRaidType);
		row.GetColumn(num++, out this.i32Scenario);
		row.GetColumn(num++, out this.i32MaxChar);
		row.GetColumn(num++, out this.i32TextExplain);
		row.GetColumn(num++, out this.nMainBossCharCode);
		row.GetColumn(num++, out this.i64BossHP);
		row.GetColumn(num++, out this.m_i16BatchMapIndex);
		row.GetColumn(num++, out this.m_fBatchMapGrideX);
		row.GetColumn(num++, out this.m_fBatchMapGrideY);
		row.GetColumn(num++, out this.m_fBatchMapGrideZ);
	}

	public string GetBossCode()
	{
		return this.nMainBossCharCode;
	}
}
