using System;
using TsLibs;

public class ItemReduceInfo : NrTableData
{
	public int iGroupUnique;

	public int iNeedItemNum;

	public int iNeedItemUnique;

	public int iNormalPoint;

	public int iSpecialPoint;

	public int iRedueceMax;

	public int iFirstNum;

	public int iFirstPoint;

	public ItemReduceInfo() : base(NrTableData.eResourceType.eRT_ITEM_REDUCE)
	{
		this.Init();
	}

	public void Init()
	{
		this.iGroupUnique = 0;
		this.iNeedItemNum = 0;
		this.iNeedItemUnique = 0;
		this.iNormalPoint = 0;
		this.iSpecialPoint = 0;
		this.iRedueceMax = 0;
		this.iFirstNum = 0;
		this.iFirstPoint = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		row.GetColumn(0, out this.iGroupUnique);
		row.GetColumn(1, out this.iNeedItemNum);
		row.GetColumn(4, out this.iNeedItemUnique);
		row.GetColumn(5, out this.iNormalPoint);
		row.GetColumn(6, out this.iSpecialPoint);
		row.GetColumn(7, out this.iRedueceMax);
		row.GetColumn(8, out this.iFirstNum);
		row.GetColumn(9, out this.iFirstPoint);
	}
}
