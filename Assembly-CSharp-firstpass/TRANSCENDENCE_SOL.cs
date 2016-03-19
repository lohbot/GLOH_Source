using System;
using TsLibs;

public class TRANSCENDENCE_SOL : NrTableData
{
	public int m_i32MaterialSeason;

	public int[] m_i32BaseSeason = new int[10];

	public TRANSCENDENCE_SOL() : base(NrTableData.eResourceType.eRT_TRANSCENDENCE_SOL)
	{
		this.Init();
	}

	public void Init()
	{
		this.m_i32MaterialSeason = 0;
		for (int i = 0; i < 10; i++)
		{
			this.m_i32BaseSeason[i] = 0;
		}
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.m_i32MaterialSeason);
		row.GetColumn(num++, out this.m_i32BaseSeason[0]);
		row.GetColumn(num++, out this.m_i32BaseSeason[1]);
		row.GetColumn(num++, out this.m_i32BaseSeason[2]);
		row.GetColumn(num++, out this.m_i32BaseSeason[3]);
		row.GetColumn(num++, out this.m_i32BaseSeason[4]);
		row.GetColumn(num++, out this.m_i32BaseSeason[5]);
		row.GetColumn(num++, out this.m_i32BaseSeason[6]);
		row.GetColumn(num++, out this.m_i32BaseSeason[7]);
		row.GetColumn(num++, out this.m_i32BaseSeason[8]);
		row.GetColumn(num++, out this.m_i32BaseSeason[9]);
	}
}
