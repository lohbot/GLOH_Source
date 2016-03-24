using System;
using TsLibs;

public class TIMESHOP_REFRESHDATA : NrTableData
{
	public short m_i16RefreshCount;

	public int m_i32ItemUnique;

	public long m_i64ItemNum;

	public TIMESHOP_REFRESHDATA()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_i16RefreshCount = 0;
		this.m_i32ItemUnique = 0;
		this.m_i64ItemNum = 0L;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.m_i16RefreshCount);
		row.GetColumn(num++, out this.m_i32ItemUnique);
		row.GetColumn(num++, out this.m_i64ItemNum);
	}
}
