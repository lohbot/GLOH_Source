using System;
using TsLibs;

public class ITEM_RATE_OPENURL_DATA : NrTableData
{
	public string strService_Code = string.Empty;

	public string strUrl = string.Empty;

	public ITEM_RATE_OPENURL_DATA()
	{
		this.Init();
	}

	public void Init()
	{
		this.strService_Code = string.Empty;
		this.strUrl = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		string empty = string.Empty;
		row.GetColumn(num++, out this.strService_Code);
		row.GetColumn(num++, out this.strUrl);
		row.GetColumn(num++, out empty);
	}
}
