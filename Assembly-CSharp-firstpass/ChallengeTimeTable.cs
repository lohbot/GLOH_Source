using System;
using TsLibs;

public class ChallengeTimeTable
{
	public short m_nUnique;

	public short m_nStartYear;

	public short m_nStartMonth;

	public short m_nStartDay;

	public short m_nStartHour;

	public short m_nStartMinute;

	public short m_nEndYear;

	public short m_nEndMonth;

	public short m_nEndDay;

	public short m_nEndHour;

	public short m_nEndMinute;

	public ChallengeTimeTable()
	{
		this.m_nUnique = 0;
		this.m_nStartYear = 0;
		this.m_nStartMonth = 0;
		this.m_nStartDay = 0;
		this.m_nStartHour = 0;
		this.m_nStartMinute = 0;
		this.m_nEndYear = 0;
		this.m_nEndMonth = 0;
		this.m_nEndDay = 0;
		this.m_nEndHour = 0;
		this.m_nEndMinute = 0;
	}

	public void SetData(TsDataReader.Row row)
	{
		int num = 0;
		row.GetColumn(num++, out this.m_nUnique);
		row.GetColumn(num++, out this.m_nStartYear);
		row.GetColumn(num++, out this.m_nStartMonth);
		row.GetColumn(num++, out this.m_nStartDay);
		row.GetColumn(num++, out this.m_nStartHour);
		row.GetColumn(num++, out this.m_nStartMinute);
		row.GetColumn(num++, out this.m_nEndYear);
		row.GetColumn(num++, out this.m_nEndMonth);
		row.GetColumn(num++, out this.m_nEndDay);
		row.GetColumn(num++, out this.m_nEndHour);
		row.GetColumn(num++, out this.m_nEndMinute);
	}
}
