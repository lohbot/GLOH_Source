using System;
using TsLibs;

public class PatchLoading_Data : NrTableData
{
	public int id;

	public string Menu_Parser = string.Empty;

	public string Menu_ex = string.Empty;

	public string strBG = string.Empty;

	public string strChar = string.Empty;

	public int iText01;

	public int iText02;

	public int iText03;

	public string strVoice01 = string.Empty;

	public string strVoice02 = string.Empty;

	public string strVoice03 = string.Empty;

	public string strPath = string.Empty;

	public PatchLoading_Data()
	{
		this.Init();
	}

	public void Init()
	{
		this.id = 0;
		this.Menu_Parser = string.Empty;
		this.Menu_ex = string.Empty;
		this.strBG = string.Empty;
		this.strChar = string.Empty;
		this.iText01 = 0;
		this.iText02 = 0;
		this.iText03 = 0;
		this.strVoice01 = string.Empty;
		this.strVoice02 = string.Empty;
		this.strVoice03 = string.Empty;
		this.strPath = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		row.GetColumn(0, out this.id);
		row.GetColumn(1, out this.Menu_Parser);
		row.GetColumn(2, out this.Menu_ex);
		row.GetColumn(3, out this.strBG);
		row.GetColumn(4, out this.strChar);
		row.GetColumn(5, out this.iText01);
		row.GetColumn(6, out this.iText02);
		row.GetColumn(7, out this.iText03);
		row.GetColumn(8, out this.strVoice01);
		row.GetColumn(9, out this.strVoice02);
		row.GetColumn(10, out this.strVoice03);
		row.GetColumn(11, out this.strPath);
	}
}
