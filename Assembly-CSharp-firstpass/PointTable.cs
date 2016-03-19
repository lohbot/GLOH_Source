using System;
using TsLibs;

public class PointTable
{
	public int m_nItemUnique;

	public int m_nGetPoint;

	public int m_nBuyPoint;

	public int m_nExchangePoint;

	public int m_nNeedItemUnique;

	public PointTable()
	{
		this.m_nItemUnique = 0;
		this.m_nGetPoint = 0;
		this.m_nBuyPoint = 0;
		this.m_nExchangePoint = 0;
		this.m_nNeedItemUnique = 0;
	}

	public void SetData(TsDataReader.Row row)
	{
		int num = 0;
		row.GetColumn(num++, out this.m_nItemUnique);
		string empty = string.Empty;
		row.GetColumn(num++, out empty);
		row.GetColumn(num++, out this.m_nGetPoint);
		row.GetColumn(num++, out this.m_nBuyPoint);
		row.GetColumn(num++, out this.m_nExchangePoint);
		row.GetColumn(num++, out this.m_nNeedItemUnique);
	}
}
