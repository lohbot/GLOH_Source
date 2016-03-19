using System;
using TsLibs;

public class MythicSolTable
{
	public int m_nItemUnique;

	public int m_nType;

	public int m_nNeedNum;

	public int m_nNeedItemUnique;

	public int m_nExchangeNum;

	public MythicSolTable()
	{
		this.m_nItemUnique = 0;
		this.m_nType = 0;
		this.m_nNeedNum = 0;
		this.m_nNeedItemUnique = 0;
		this.m_nExchangeNum = 0;
	}

	public void SetData(TsDataReader.Row row)
	{
		int num = 0;
		row.GetColumn(num++, out this.m_nItemUnique);
		string empty = string.Empty;
		row.GetColumn(num++, out empty);
		row.GetColumn(num++, out this.m_nType);
		row.GetColumn(num++, out this.m_nNeedNum);
		row.GetColumn(num++, out this.m_nNeedItemUnique);
		row.GetColumn(num++, out this.m_nExchangeNum);
	}
}
