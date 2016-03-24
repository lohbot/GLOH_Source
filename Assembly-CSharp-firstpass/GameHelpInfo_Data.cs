using System;
using TsLibs;

public class GameHelpInfo_Data : NrTableData
{
	public short m_nSort;

	public string m_HelpListName = string.Empty;

	public string m_strTitle = string.Empty;

	public byte m_byListOnOff;

	public string[] m_strText;

	public string[] m_strTexture;

	public byte[] m_byPageOnOff;

	public GameHelpInfo_Data()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_nSort = 0;
		this.m_HelpListName = string.Empty;
		this.m_strTitle = string.Empty;
		this.m_strText = new string[10];
		this.m_strTexture = new string[10];
		this.m_byPageOnOff = new byte[10];
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.m_nSort);
		row.GetColumn(num++, out this.m_HelpListName);
		row.GetColumn(num++, out this.m_strTitle);
		row.GetColumn(num++, out this.m_byListOnOff);
		row.GetColumn(num++, out this.m_strText[0]);
		row.GetColumn(num++, out this.m_strTexture[0]);
		row.GetColumn(num++, out this.m_byPageOnOff[0]);
		row.GetColumn(num++, out this.m_strText[1]);
		row.GetColumn(num++, out this.m_strTexture[1]);
		row.GetColumn(num++, out this.m_byPageOnOff[1]);
		row.GetColumn(num++, out this.m_strText[2]);
		row.GetColumn(num++, out this.m_strTexture[2]);
		row.GetColumn(num++, out this.m_byPageOnOff[2]);
		row.GetColumn(num++, out this.m_strText[3]);
		row.GetColumn(num++, out this.m_strTexture[3]);
		row.GetColumn(num++, out this.m_byPageOnOff[3]);
		row.GetColumn(num++, out this.m_strText[4]);
		row.GetColumn(num++, out this.m_strTexture[4]);
		row.GetColumn(num++, out this.m_byPageOnOff[4]);
		row.GetColumn(num++, out this.m_strText[5]);
		row.GetColumn(num++, out this.m_strTexture[5]);
		row.GetColumn(num++, out this.m_byPageOnOff[5]);
		row.GetColumn(num++, out this.m_strText[6]);
		row.GetColumn(num++, out this.m_strTexture[6]);
		row.GetColumn(num++, out this.m_byPageOnOff[6]);
		row.GetColumn(num++, out this.m_strText[7]);
		row.GetColumn(num++, out this.m_strTexture[7]);
		row.GetColumn(num++, out this.m_byPageOnOff[7]);
		row.GetColumn(num++, out this.m_strText[8]);
		row.GetColumn(num++, out this.m_strTexture[8]);
		row.GetColumn(num++, out this.m_byPageOnOff[8]);
		row.GetColumn(num++, out this.m_strText[9]);
		row.GetColumn(num++, out this.m_strTexture[9]);
		row.GetColumn(num++, out this.m_byPageOnOff[9]);
	}
}
