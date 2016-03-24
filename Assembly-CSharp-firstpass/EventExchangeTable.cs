using System;
using TsLibs;

public class EventExchangeTable
{
	public int m_nIDX;

	public int m_nItemUnique;

	public int m_nItemNum;

	public int[] m_nNeedItemUnique = new int[3];

	public int[] m_nNeedItemCount = new int[3];

	public int m_nExchangeLimit;

	public EventExchangeTable()
	{
		this.m_nIDX = 0;
		this.m_nItemUnique = 0;
		for (int i = 0; i < 3; i++)
		{
			this.m_nNeedItemUnique[i] = 0;
			this.m_nNeedItemCount[i] = 0;
		}
		this.m_nExchangeLimit = 0;
	}

	public void SetData(TsDataReader.Row row)
	{
		int num = 0;
		row.GetColumn(num++, out this.m_nIDX);
		row.GetColumn(num++, out this.m_nItemUnique);
		string empty = string.Empty;
		row.GetColumn(num++, out empty);
		row.GetColumn(num++, out this.m_nItemNum);
		row.GetColumn(num++, out this.m_nNeedItemUnique[0]);
		row.GetColumn(num++, out this.m_nNeedItemCount[0]);
		row.GetColumn(num++, out this.m_nNeedItemUnique[1]);
		row.GetColumn(num++, out this.m_nNeedItemCount[1]);
		row.GetColumn(num++, out this.m_nNeedItemUnique[2]);
		row.GetColumn(num++, out this.m_nNeedItemCount[2]);
		row.GetColumn(num++, out this.m_nExchangeLimit);
	}
}
