using System;
using TsLibs;

public class SolWarehouseInfo : NrTableData
{
	public int iWarehouseNumber;

	public long iNeedMoney;

	public int iNeedHeartsNum;

	public SolWarehouseInfo() : base(NrTableData.eResourceType.eRT_SOLWAREHOUSE)
	{
		this.Init();
	}

	public void Init()
	{
		this.iWarehouseNumber = 0;
		this.iNeedMoney = 0L;
		this.iNeedHeartsNum = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		row.GetColumn(0, out this.iWarehouseNumber);
		row.GetColumn(1, out this.iNeedMoney);
		row.GetColumn(2, out this.iNeedHeartsNum);
	}
}
