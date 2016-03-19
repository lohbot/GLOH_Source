using System;
using TsLibs;

public class TRANSCENDENCE_COST : NrTableData
{
	public int m_i32Season;

	public long[] m_i64Cost = new long[4];

	public TRANSCENDENCE_COST() : base(NrTableData.eResourceType.eRT_TRANSCENDENCE_COST)
	{
		this.Init();
	}

	public void Init()
	{
		this.m_i32Season = 0;
		for (int i = 0; i < 4; i++)
		{
			this.m_i64Cost[i] = 0L;
		}
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.m_i32Season);
		row.GetColumn(num++, out this.m_i64Cost[0]);
		row.GetColumn(num++, out this.m_i64Cost[1]);
		row.GetColumn(num++, out this.m_i64Cost[2]);
		row.GetColumn(num++, out this.m_i64Cost[3]);
	}
}
