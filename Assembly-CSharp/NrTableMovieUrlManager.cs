using System;
using System.Collections.Generic;
using TsLibs;

public class NrTableMovieUrlManager : NrTSingleton<NrTableMovieUrlManager>
{
	private Dictionary<int, Table_MovieUrl_Data> m_dicMovieUrl = new Dictionary<int, Table_MovieUrl_Data>();

	private NrTableMovieUrlManager()
	{
	}

	public void SetData(TsDataReader.Row row)
	{
		Table_MovieUrl_Data table_MovieUrl_Data = new Table_MovieUrl_Data();
		table_MovieUrl_Data.SetData(row);
		this.m_dicMovieUrl.Add(table_MovieUrl_Data.m_iMovieUnique, table_MovieUrl_Data);
	}

	public string GetMovieUrl(int movieUnique)
	{
		if (this.m_dicMovieUrl == null || !this.m_dicMovieUrl.ContainsKey(movieUnique) || this.m_dicMovieUrl[movieUnique] == null)
		{
			return string.Empty;
		}
		return this.m_dicMovieUrl[movieUnique].m_strMovieUrl;
	}
}
