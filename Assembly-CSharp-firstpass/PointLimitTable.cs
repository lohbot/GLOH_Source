using System;
using TsLibs;

public class PointLimitTable
{
	public int m_nLevel;

	public int m_nHeroTicketNum;

	public int m_nEquipTicketNum;

	public PointLimitTable()
	{
		this.m_nLevel = 0;
		this.m_nHeroTicketNum = 0;
		this.m_nEquipTicketNum = 0;
	}

	public void SetData(TsDataReader.Row row)
	{
		int num = 0;
		row.GetColumn(num++, out this.m_nLevel);
		row.GetColumn(num++, out this.m_nHeroTicketNum);
		row.GetColumn(num++, out this.m_nEquipTicketNum);
	}
}
