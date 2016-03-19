using GAME;
using System;
using TsLibs;

public class ITEMTYPE_INFO : NrTableData
{
	public eITEM_PART ITEMPART;

	public int ITEMTYPE;

	public string ITEMTYPECODE = string.Empty;

	public string TEXTKEY = string.Empty;

	public long ATB;

	public int ATTACKTYPE;

	public int WEAPONTYPE;

	public long EQUIPCLASSTYPE;

	public int OPTION1;

	public int OPTION2;

	public int AuctionSearch;

	public string szItemPart = string.Empty;

	public string szATB = string.Empty;

	public string szAttackTypeCode = string.Empty;

	public string szClassTypeCode = string.Empty;

	public string szOption1 = string.Empty;

	public string szOption2 = string.Empty;

	public ITEMTYPE_INFO() : base(NrTableData.eResourceType.eRT_ITEMTYPE_INFO)
	{
		this.Init();
	}

	public void Init()
	{
		this.ITEMPART = eITEM_PART.ITEMPART_NONE;
		this.ITEMTYPE = 0;
		this.ITEMTYPECODE = string.Empty;
		this.TEXTKEY = string.Empty;
		this.ATB = 0L;
		this.ATTACKTYPE = 0;
		this.WEAPONTYPE = 0;
		this.EQUIPCLASSTYPE = 0L;
		this.OPTION1 = 0;
		this.OPTION2 = 0;
		this.AuctionSearch = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.szItemPart);
		row.GetColumn(num++, out this.ITEMTYPECODE);
		row.GetColumn(num++, out this.TEXTKEY);
		row.GetColumn(num++, out this.szATB);
		row.GetColumn(num++, out this.szAttackTypeCode);
		row.GetColumn(num++, out this.szClassTypeCode);
		row.GetColumn(num++, out this.szOption1);
		row.GetColumn(num++, out this.szOption2);
		row.GetColumn(num++, out this.AuctionSearch);
	}
}
