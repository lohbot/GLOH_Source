using System;
using TsLibs;

public class PLUNDER_SUPPORT_GOLD : NrTableData
{
	public int m_nCharLevel;

	public long m_nSupportGold;

	public long m_nMaxGold;

	public PLUNDER_SUPPORT_GOLD()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_nCharLevel = 0;
		this.m_nSupportGold = 0L;
		this.m_nMaxGold = 0L;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.m_nCharLevel);
		row.GetColumn(num++, out this.m_nSupportGold);
		row.GetColumn(num++, out this.m_nMaxGold);
	}
}
