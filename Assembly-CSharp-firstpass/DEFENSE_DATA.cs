using System;
using TsLibs;

public class DEFENSE_DATA : NrTableData
{
	public int m_nIDX;

	public int m_nDAMAGE_DECREASE;

	public int m_nDEFENSE_VALUE;

	public float m_fDAMAGE_RATE;

	public void Init()
	{
		this.m_nIDX = 0;
		this.m_nDAMAGE_DECREASE = 0;
		this.m_nDEFENSE_VALUE = 0;
		this.m_fDAMAGE_RATE = 0f;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.m_nIDX);
		row.GetColumn(num++, out this.m_nDAMAGE_DECREASE);
		row.GetColumn(num++, out this.m_nDEFENSE_VALUE);
		row.GetColumn(num++, out this.m_fDAMAGE_RATE);
	}
}
