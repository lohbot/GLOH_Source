using System;
using TsLibs;

public class ITEM_SUPPLIES : NrTableData
{
	public int ITEMUNIQUE;

	public string TYPECODE = string.Empty;

	public string TEXTKEY = string.Empty;

	public string ENG_NAME = string.Empty;

	public string ATB = string.Empty;

	public int USE_MINLV;

	public int USE_MAXLV;

	public byte FUNCTIONS;

	public int PARAM1;

	public int PARAM2;

	public int USEDATE;

	public string TEXTKEY_TOOLTIP = string.Empty;

	public int SORT_ORDER;

	public long PRICE;

	public string MATERIALCODE = string.Empty;

	public string m_strIconFile = string.Empty;

	public short m_shIconIndex;

	public ITEM_SUPPLIES() : base(NrTableData.eResourceType.eRT_ITEM_SUPPLIES)
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
		this.USE_MINLV = 0;
		this.USE_MAXLV = 0;
		this.FUNCTIONS = 0;
		this.PARAM1 = 0;
		this.PARAM2 = 0;
		this.USEDATE = 0;
		this.TEXTKEY_TOOLTIP = string.Empty;
		this.SORT_ORDER = 0;
		this.PRICE = 0L;
		this.MATERIALCODE = string.Empty;
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
		row.GetColumn(num++, out this.USE_MINLV);
		row.GetColumn(num++, out this.USE_MAXLV);
		row.GetColumn(num++, out this.FUNCTIONS);
		row.GetColumn(num++, out this.PARAM1);
		row.GetColumn(num++, out this.PARAM2);
		row.GetColumn(num++, out this.USEDATE);
		row.GetColumn(num++, out this.TEXTKEY_TOOLTIP);
		row.GetColumn(num++, out this.SORT_ORDER);
		row.GetColumn(num++, out this.PRICE);
		row.GetColumn(num++, out this.MATERIALCODE);
		row.GetColumn(num++, out this.m_strIconFile);
		row.GetColumn(num++, out this.m_shIconIndex);
		if (this.FUNCTIONS == 8)
		{
			this.PARAM2 = Math.Max(0, this.PARAM2 - 1);
		}
	}
}
