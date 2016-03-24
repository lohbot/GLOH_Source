using System;
using TsLibs;

public class VipSubInfo : NrTableData
{
	public byte byVipLevel;

	public string strTitle = string.Empty;

	public string strState = string.Empty;

	public string strNote = string.Empty;

	public string strIconPath = string.Empty;

	public VipSubInfo()
	{
		this.Init();
	}

	public void Init()
	{
		this.byVipLevel = 0;
		this.strTitle = string.Empty;
		this.strState = string.Empty;
		this.strNote = string.Empty;
		this.strIconPath = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.byVipLevel);
		row.GetColumn(num++, out this.strTitle);
		row.GetColumn(num++, out this.strState);
		row.GetColumn(num++, out this.strNote);
		row.GetColumn(num++, out this.strIconPath);
	}
}
