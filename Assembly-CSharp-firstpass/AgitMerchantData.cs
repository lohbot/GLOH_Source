using System;
using TsLibs;

public class AgitMerchantData : NrTableData
{
	public short i16SellType;

	public int i32ItemUnique;

	public int i32ItemNum;

	public int i32MinPrice;

	public int i32MaxPrice;

	public AgitMerchantData() : base(NrTableData.eResourceType.eRT_AGIT_MERCHNAT)
	{
		this.Init();
	}

	public void Init()
	{
		this.i16SellType = 0;
		this.i32ItemUnique = 0;
		this.i32ItemNum = 0;
		this.i32MinPrice = 0;
		this.i32MaxPrice = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		row.GetColumn(0, out this.i16SellType);
		row.GetColumn(1, out this.i32ItemUnique);
		row.GetColumn(2, out this.i32ItemNum);
		row.GetColumn(4, out this.i32MinPrice);
		row.GetColumn(5, out this.i32MaxPrice);
	}
}
