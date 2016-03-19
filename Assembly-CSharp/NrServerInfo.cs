using System;
using TsLibs;

public class NrServerInfo
{
	public string m_strServerName = string.Empty;

	public string m_strLoginServerIP = string.Empty;

	public string m_strLoginServerPort = string.Empty;

	public string m_strWorldServerIP = string.Empty;

	public string m_strWorldServerPort = string.Empty;

	public void SetData(TsDataReader.Row row)
	{
		int num = 0;
		row.GetColumn(num++, out this.m_strServerName);
		row.GetColumn(num++, out this.m_strLoginServerIP);
		row.GetColumn(num++, out this.m_strLoginServerPort);
		row.GetColumn(num++, out this.m_strWorldServerIP);
		row.GetColumn(num++, out this.m_strWorldServerPort);
	}
}
