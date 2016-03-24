using System;
using TsLibs;

public class GuildWarExchangeTable
{
	public int m_ItemIndex;

	public int m_nItemUnique;

	public int m_nNeedItemUnique;

	public int m_nNeedNum;

	public int m_nExchangeLimit;

	public GuildWarExchangeTable()
	{
		this.m_ItemIndex = 0;
		this.m_nItemUnique = 0;
		this.m_nNeedItemUnique = 0;
		this.m_nNeedNum = 0;
		this.m_nExchangeLimit = 0;
	}

	public void SetData(TsDataReader.Row row)
	{
		int num = 0;
		row.GetColumn(num++, out this.m_ItemIndex);
		row.GetColumn(num++, out this.m_nItemUnique);
		string empty = string.Empty;
		row.GetColumn(num++, out empty);
		row.GetColumn(num++, out this.m_nNeedItemUnique);
		row.GetColumn(num++, out this.m_nNeedNum);
		row.GetColumn(num++, out this.m_nExchangeLimit);
	}
}
