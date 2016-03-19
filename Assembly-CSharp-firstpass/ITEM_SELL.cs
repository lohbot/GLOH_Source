using System;
using TsLibs;

public class ITEM_SELL : NrTableData
{
	public int nGroupUnique;

	public int nItemMakeRank;

	public int nItemSellMoney;

	public string stRank = string.Empty;

	public ITEM_SELL()
	{
		this.Init();
	}

	public void Init()
	{
		this.nGroupUnique = 0;
		this.nItemMakeRank = 0;
		this.nItemSellMoney = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.nGroupUnique);
		row.GetColumn(num++, out this.stRank);
		row.GetColumn(num++, out this.nItemSellMoney);
	}
}
