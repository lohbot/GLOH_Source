using System;
using TsLibs;

public class Table_MovieUrl_Data : NrTableData
{
	public int m_iMovieUnique;

	public string m_strMovieUrl = string.Empty;

	public override void SetData(TsDataReader.Row row)
	{
		int num = 0;
		row.GetColumn(num++, out this.m_iMovieUnique);
		row.GetColumn(num++, out this.m_strMovieUrl);
	}
}
