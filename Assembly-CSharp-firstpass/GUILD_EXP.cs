using System;
using TsLibs;

public class GUILD_EXP : NrTableData
{
	public short m_nLevel;

	public long m_nExp;

	public GUILD_EXP()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_nLevel = 0;
		this.m_nExp = 0L;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.m_nLevel);
		row.GetColumn(num++, out this.m_nExp);
	}
}
