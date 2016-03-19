using System;
using TsLibs;

public class TRANSCENDENCE_FAILREWARD : NrTableData
{
	public int m_i32Season;

	public short[] m_i16ItemNum = new short[4];

	public TRANSCENDENCE_FAILREWARD() : base(NrTableData.eResourceType.eRT_TRANSCENDENCE_FAILREWARD)
	{
		this.Init();
	}

	public void Init()
	{
		this.m_i32Season = 0;
		for (int i = 0; i < 4; i++)
		{
			this.m_i16ItemNum[i] = 0;
		}
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.m_i32Season);
		row.GetColumn(num++, out this.m_i16ItemNum[0]);
		row.GetColumn(num++, out this.m_i16ItemNum[1]);
		row.GetColumn(num++, out this.m_i16ItemNum[2]);
		row.GetColumn(num++, out this.m_i16ItemNum[3]);
	}
}
