using System;
using TsLibs;

public class ITEM_BOX : NrTableData
{
	public int ITEMUNIQUE;

	public string TYPECODE = string.Empty;

	public string TEXTKEY = string.Empty;

	public string ENG_NAME = string.Empty;

	public string ATB = string.Empty;

	public int USE_MINLV;

	public int USE_MAXLV;

	public int ITEMUNIQUE1;

	public int ITEMNUM1;

	public int ITEMPROB1;

	public int ITEMUNIQUE2;

	public int ITEMNUM2;

	public int ITEMPROB2;

	public int ITEMUNIQUE3;

	public int ITEMNUM3;

	public int ITEMPROB3;

	public int ITEMUNIQUE4;

	public int ITEMNUM4;

	public int ITEMPROB4;

	public int ITEMUNIQUE5;

	public int ITEMNUM5;

	public int ITEMPROB5;

	public int ITEMUNIQUE6;

	public int ITEMNUM6;

	public int ITEMPROB6;

	public int ITEMUNIQUE7;

	public int ITEMNUM7;

	public int ITEMPROB7;

	public int ITEMUNIQUE8;

	public int ITEMNUM8;

	public int ITEMPROB8;

	public int ITEMUNIQUE9;

	public int ITEMNUM9;

	public int ITEMPROB9;

	public int ITEMUNIQUE10;

	public int ITEMNUM10;

	public int ITEMPROB10;

	public int ITEMUNIQUE11;

	public int ITEMNUM11;

	public int ITEMPROB11;

	public int ITEMUNIQUE12;

	public int ITEMNUM12;

	public int ITEMPROB12;

	public int USEDATE;

	public string TEXTKEY_TOOLTIP = string.Empty;

	public int SORT_ORDER;

	public long PRICE;

	public string MaterialCode = string.Empty;

	public string m_strIconFile = string.Empty;

	public short m_shIconIndex;

	public int BOXRANK;

	public int BOXSEALINFO;

	public int NEEDOPENITEMUNIQUE;

	public int NEEDOPENITEMNUM;

	public ITEM_BOX() : base(NrTableData.eResourceType.eRT_ITEM_BOX)
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
		this.ITEMUNIQUE1 = 0;
		this.ITEMNUM1 = 0;
		this.ITEMPROB1 = 0;
		this.ITEMUNIQUE2 = 0;
		this.ITEMNUM2 = 0;
		this.ITEMPROB2 = 0;
		this.ITEMUNIQUE3 = 0;
		this.ITEMNUM3 = 0;
		this.ITEMPROB3 = 0;
		this.ITEMUNIQUE4 = 0;
		this.ITEMNUM4 = 0;
		this.ITEMPROB4 = 0;
		this.ITEMUNIQUE5 = 0;
		this.ITEMNUM5 = 0;
		this.ITEMPROB5 = 0;
		this.ITEMUNIQUE6 = 0;
		this.ITEMNUM6 = 0;
		this.ITEMPROB6 = 0;
		this.ITEMUNIQUE7 = 0;
		this.ITEMNUM7 = 0;
		this.ITEMPROB7 = 0;
		this.ITEMUNIQUE8 = 0;
		this.ITEMNUM8 = 0;
		this.ITEMPROB8 = 0;
		this.ITEMUNIQUE9 = 0;
		this.ITEMNUM9 = 0;
		this.ITEMPROB9 = 0;
		this.ITEMUNIQUE10 = 0;
		this.ITEMNUM10 = 0;
		this.ITEMPROB10 = 0;
		this.ITEMUNIQUE11 = 0;
		this.ITEMNUM11 = 0;
		this.ITEMPROB11 = 0;
		this.ITEMUNIQUE12 = 0;
		this.ITEMNUM12 = 0;
		this.ITEMPROB12 = 0;
		this.USEDATE = 0;
		this.TEXTKEY_TOOLTIP = string.Empty;
		this.SORT_ORDER = 0;
		this.PRICE = 0L;
		this.MaterialCode = string.Empty;
		this.m_strIconFile = string.Empty;
		this.m_shIconIndex = 0;
		this.BOXRANK = 0;
		this.BOXSEALINFO = 0;
		this.NEEDOPENITEMUNIQUE = 0;
		this.NEEDOPENITEMNUM = 0;
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
		row.GetColumn(num++, out this.ITEMUNIQUE1);
		row.GetColumn(num++, out this.ITEMNUM1);
		row.GetColumn(num++, out this.ITEMPROB1);
		row.GetColumn(num++, out this.ITEMUNIQUE2);
		row.GetColumn(num++, out this.ITEMNUM2);
		row.GetColumn(num++, out this.ITEMPROB2);
		row.GetColumn(num++, out this.ITEMUNIQUE3);
		row.GetColumn(num++, out this.ITEMNUM3);
		row.GetColumn(num++, out this.ITEMPROB3);
		row.GetColumn(num++, out this.ITEMUNIQUE4);
		row.GetColumn(num++, out this.ITEMNUM4);
		row.GetColumn(num++, out this.ITEMPROB4);
		row.GetColumn(num++, out this.ITEMUNIQUE5);
		row.GetColumn(num++, out this.ITEMNUM5);
		row.GetColumn(num++, out this.ITEMPROB5);
		row.GetColumn(num++, out this.ITEMUNIQUE6);
		row.GetColumn(num++, out this.ITEMNUM6);
		row.GetColumn(num++, out this.ITEMPROB6);
		row.GetColumn(num++, out this.ITEMUNIQUE7);
		row.GetColumn(num++, out this.ITEMNUM7);
		row.GetColumn(num++, out this.ITEMPROB7);
		row.GetColumn(num++, out this.ITEMUNIQUE8);
		row.GetColumn(num++, out this.ITEMNUM8);
		row.GetColumn(num++, out this.ITEMPROB8);
		row.GetColumn(num++, out this.ITEMUNIQUE9);
		row.GetColumn(num++, out this.ITEMNUM9);
		row.GetColumn(num++, out this.ITEMPROB9);
		row.GetColumn(num++, out this.ITEMUNIQUE10);
		row.GetColumn(num++, out this.ITEMNUM10);
		row.GetColumn(num++, out this.ITEMPROB10);
		row.GetColumn(num++, out this.ITEMUNIQUE11);
		row.GetColumn(num++, out this.ITEMNUM11);
		row.GetColumn(num++, out this.ITEMPROB11);
		row.GetColumn(num++, out this.ITEMUNIQUE12);
		row.GetColumn(num++, out this.ITEMNUM12);
		row.GetColumn(num++, out this.ITEMPROB12);
		row.GetColumn(num++, out this.USEDATE);
		row.GetColumn(num++, out this.TEXTKEY_TOOLTIP);
		row.GetColumn(num++, out this.SORT_ORDER);
		row.GetColumn(num++, out this.PRICE);
		row.GetColumn(num++, out this.MaterialCode);
		row.GetColumn(num++, out this.m_strIconFile);
		row.GetColumn(num++, out this.m_shIconIndex);
		row.GetColumn(num++, out this.BOXRANK);
		row.GetColumn(num++, out this.BOXSEALINFO);
		row.GetColumn(num++, out this.NEEDOPENITEMUNIQUE);
		row.GetColumn(num++, out this.NEEDOPENITEMNUM);
	}
}
