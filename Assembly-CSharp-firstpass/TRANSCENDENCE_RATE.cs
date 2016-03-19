using System;
using TsLibs;

public class TRANSCENDENCE_RATE : NrTableData
{
	public byte m_bGrade;

	public short m_i16BassGrade_A;

	public short m_i16BassGrade_S;

	public short m_i16BassGrade_SS;

	public TRANSCENDENCE_RATE() : base(NrTableData.eResourceType.eRT_TRANSCENDENCE_RATE)
	{
		this.Init();
	}

	public void Init()
	{
		this.m_bGrade = 0;
		this.m_i16BassGrade_A = 0;
		this.m_i16BassGrade_S = 0;
		this.m_i16BassGrade_SS = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.m_bGrade);
		row.GetColumn(num++, out this.m_i16BassGrade_A);
		row.GetColumn(num++, out this.m_i16BassGrade_S);
		row.GetColumn(num++, out this.m_i16BassGrade_SS);
	}
}
