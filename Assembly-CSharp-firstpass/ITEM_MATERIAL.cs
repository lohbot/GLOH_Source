using System;
using TsLibs;

public class ITEM_MATERIAL : NrTableData
{
	public int ITEMUNIQUE;

	public string TYPECODE = string.Empty;

	public string TEXTKEY = string.Empty;

	public string ENG_NAME = string.Empty;

	public string ATB = string.Empty;

	public int PRUDUCT_IDX;

	public int GROUP_IDX;

	public int USEDATE;

	public string TEXTKEY_TOOLTIP = string.Empty;

	public int SORT_ORDER;

	public long PRICE;

	public string MATERIALCODE = string.Empty;

	public string TEXT_COLOR_CODE = string.Empty;

	public string m_strIconFile = string.Empty;

	public short m_shIconIndex;

	public ITEM_MATERIAL() : base(NrTableData.eResourceType.eRT_ITEM_MATERIAL)
	{
		this.Init();
	}

	public void Init()
	{
		this.ITEMUNIQUE = 0;
		this.TYPECODE = string.Empty;
		this.TEXTKEY = string.Empty;
		this.ENG_NAME = string.Empty;
		this.ATB = string.Empty;
		this.PRUDUCT_IDX = 0;
		this.GROUP_IDX = 0;
		this.USEDATE = 0;
		this.TEXTKEY_TOOLTIP = string.Empty;
		this.SORT_ORDER = 0;
		this.PRICE = 0L;
		this.MATERIALCODE = string.Empty;
		this.TEXT_COLOR_CODE = string.Empty;
		this.m_strIconFile = string.Empty;
		this.m_shIconIndex = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.ITEMUNIQUE);
		row.GetColumn(num++, out this.TYPECODE);
		row.GetColumn(num++, out this.TEXTKEY);
		row.GetColumn(num++, out this.ENG_NAME);
		row.GetColumn(num++, out this.ATB);
		row.GetColumn(num++, out this.PRUDUCT_IDX);
		row.GetColumn(num++, out this.GROUP_IDX);
		row.GetColumn(num++, out this.USEDATE);
		row.GetColumn(num++, out this.TEXTKEY_TOOLTIP);
		row.GetColumn(num++, out this.SORT_ORDER);
		row.GetColumn(num++, out this.PRICE);
		row.GetColumn(num++, out this.MATERIALCODE);
		row.GetColumn(num++, out this.TEXT_COLOR_CODE);
		row.GetColumn(num++, out this.m_strIconFile);
		row.GetColumn(num++, out this.m_shIconIndex);
	}
}
