using System;
using TsLibs;

public class GMHELP_INFO : NrTableData
{
	public byte m_bGMKind;

	public int m_i32TextKey;

	public string m_strGMPortraitFile = string.Empty;

	public GMHELP_INFO() : base(NrTableData.eResourceType.eRT_GMHELPINFO)
	{
		this.Init();
	}

	public void Init()
	{
		this.m_bGMKind = 0;
		this.m_i32TextKey = 0;
		this.m_strGMPortraitFile = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.m_bGMKind);
		row.GetColumn(num++, out this.m_i32TextKey);
		row.GetColumn(num++, out this.m_strGMPortraitFile);
	}
}
