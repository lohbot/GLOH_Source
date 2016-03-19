using System;
using TsLibs;

public class CHARKIND_CLASSINFO : NrTableData
{
	public int CLASSINDEX;

	public long CLASSTYPE;

	public string CLASSCODE = string.Empty;

	public string TEXTKEY = string.Empty;

	public byte CHARCLASS;

	public string EXP_TYPE = string.Empty;

	public string TEXTKEY_DESC = string.Empty;

	public CHARKIND_CLASSINFO() : base(NrTableData.eResourceType.eRT_CHARKIND_CLASSINFO)
	{
		this.Init();
	}

	public void Init()
	{
		this.CLASSINDEX = 0;
		this.CLASSTYPE = 0L;
		this.CLASSCODE = string.Empty;
		this.TEXTKEY = string.Empty;
		this.CHARCLASS = 0;
		this.EXP_TYPE = string.Empty;
		this.TEXTKEY_DESC = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.CLASSINDEX);
		row.GetColumn(num++, out this.CLASSCODE);
		row.GetColumn(num++, out this.TEXTKEY);
		row.GetColumn(num++, out this.CHARCLASS);
		row.GetColumn(num++, out this.EXP_TYPE);
		row.GetColumn(num++, out this.TEXTKEY_DESC);
	}
}
