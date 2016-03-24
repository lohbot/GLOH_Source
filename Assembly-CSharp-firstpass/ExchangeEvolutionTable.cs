using System;
using TsLibs;

public class ExchangeEvolutionTable
{
	public int m_nIDX;

	public int m_nItemUnique;

	public string m_cItemName = string.Empty;

	public int m_nItemCount;

	public int m_nNeedItemUnique;

	public int m_nNeedCount;

	public ExchangeEvolutionTable()
	{
		this.m_nIDX = 0;
		this.m_nItemUnique = 0;
		this.m_cItemName = string.Empty;
		this.m_nItemCount = 0;
		this.m_nNeedItemUnique = 0;
		this.m_nNeedCount = 0;
	}

	public void SetData(TsDataReader.Row row)
	{
		int num = 0;
		row.GetColumn(num++, out this.m_nIDX);
		row.GetColumn(num++, out this.m_nItemUnique);
		row.GetColumn(num++, out this.m_cItemName);
		row.GetColumn(num++, out this.m_nItemCount);
		row.GetColumn(num++, out this.m_nNeedItemUnique);
		row.GetColumn(num++, out this.m_nNeedCount);
	}
}
