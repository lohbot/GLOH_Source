using System;
using TsLibs;

public class ITEM_QUEST : NrTableData
{
	public int ITEMUNIQUE;

	public string TYPECODE = string.Empty;

	public string TEXTKEY = string.Empty;

	public string ENG_NAME = string.Empty;

	public string ATB = string.Empty;

	public string LINK_QUEST1 = string.Empty;

	public string LINK_QUEST2 = string.Empty;

	public string LINK_QUEST3 = string.Empty;

	public byte IS_USE;

	public int CALL_MOB;

	public int CALL_MOBAREA;

	public int PRUDUCT_IDX;

	public int USEDATE;

	public string TEXTKEY_TOOLTIP = string.Empty;

	public int SORT_ORDER;

	public long PRICE;

	public string MATERIALCODE = string.Empty;

	public int ItemFunc;

	public int FuncParam;

	public byte IsDrop;

	public byte IsDisappear;

	public string m_strIconFile = string.Empty;

	public short m_shIconIndex;

	public ITEM_QUEST() : base(NrTableData.eResourceType.eRT_ITEM_QUEST)
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
		this.LINK_QUEST1 = string.Empty;
		this.LINK_QUEST2 = string.Empty;
		this.LINK_QUEST3 = string.Empty;
		this.IS_USE = 0;
		this.CALL_MOB = 0;
		this.CALL_MOBAREA = 0;
		this.PRUDUCT_IDX = 0;
		this.USEDATE = 0;
		this.TEXTKEY_TOOLTIP = string.Empty;
		this.SORT_ORDER = 0;
		this.PRICE = 0L;
		this.MATERIALCODE = string.Empty;
		this.ItemFunc = 0;
		this.FuncParam = 0;
		this.IsDrop = 0;
		this.IsDisappear = 0;
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
		row.GetColumn(num++, out this.LINK_QUEST1);
		row.GetColumn(num++, out this.LINK_QUEST2);
		row.GetColumn(num++, out this.LINK_QUEST3);
		row.GetColumn(num++, out this.CALL_MOB);
		row.GetColumn(num++, out this.CALL_MOBAREA);
		row.GetColumn(num++, out this.PRUDUCT_IDX);
		row.GetColumn(num++, out this.USEDATE);
		row.GetColumn(num++, out this.TEXTKEY_TOOLTIP);
		row.GetColumn(num++, out this.SORT_ORDER);
		row.GetColumn(num++, out this.PRICE);
		row.GetColumn(num++, out this.MATERIALCODE);
		row.GetColumn(num++, out this.ItemFunc);
		row.GetColumn(num++, out this.FuncParam);
		row.GetColumn(num++, out this.m_strIconFile);
		row.GetColumn(num++, out this.m_shIconIndex);
	}
}
