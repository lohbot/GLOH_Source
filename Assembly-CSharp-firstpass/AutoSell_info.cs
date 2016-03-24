using System;
using TsLibs;

public class AutoSell_info : NrTableData
{
	public int i32SellNumber;

	public int i32ItemGroupUnique;

	public int i32ItemTextKey;

	public string szTextName = string.Empty;

	public AutoSell_info() : base(NrTableData.eResourceType.eRT_AUTOSELL)
	{
		this.Init();
	}

	public void Init()
	{
		this.i32SellNumber = 0;
		this.i32ItemGroupUnique = 0;
		this.i32ItemTextKey = 0;
		this.szTextName = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.i32SellNumber);
		row.GetColumn(num++, out this.i32ItemGroupUnique);
		row.GetColumn(num++, out this.i32ItemTextKey);
		row.GetColumn(num++, out this.szTextName);
	}
}
