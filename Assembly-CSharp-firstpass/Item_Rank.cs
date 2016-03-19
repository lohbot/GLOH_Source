using System;
using TsLibs;

public class Item_Rank : NrTableData
{
	public int ItemRank;

	public float ItemPriceRate;

	public int MaxSuccessProb;

	public int ItemPerformanceRate;

	public int MakeProb;

	public int QuailtyLevel;

	public int ItemUnique;

	public long ItemPrice;

	public Item_Rank()
	{
		this.Init();
	}

	public void Init()
	{
		this.ItemRank = 0;
		this.ItemPriceRate = 0f;
		this.MaxSuccessProb = 0;
		this.ItemPerformanceRate = 0;
		this.MakeProb = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.ItemRank);
		row.GetColumn(num++, out this.ItemPriceRate);
		row.GetColumn(num++, out this.MaxSuccessProb);
		row.GetColumn(num++, out this.ItemPerformanceRate);
		row.GetColumn(num++, out this.MakeProb);
		row.GetColumn(num++, out this.QuailtyLevel);
		row.GetColumn(num++, out this.ItemUnique);
		row.GetColumn(num++, out this.ItemPrice);
	}
}
