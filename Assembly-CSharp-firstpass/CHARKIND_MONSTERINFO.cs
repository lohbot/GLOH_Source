using System;
using TsLibs;

public class CHARKIND_MONSTERINFO : NrTableData
{
	public string CharCode = string.Empty;

	public short MonType;

	public short MINLEVEL;

	public short MAXLEVEL;

	public short ReputeUnique;

	public int ReputeValue;

	public string DROPITEM = string.Empty;

	public CHARKIND_MONSTERINFO() : base(NrTableData.eResourceType.eRT_CHARKIND_MONSTERINFO)
	{
		this.Init();
	}

	public void Init()
	{
		this.CharCode = string.Empty;
		this.MonType = 0;
		this.MINLEVEL = 0;
		this.MAXLEVEL = 0;
		this.ReputeUnique = 0;
		this.ReputeValue = 0;
		this.DROPITEM = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.CharCode);
		row.GetColumn(num++, out this.MonType);
		row.GetColumn(num++, out this.MINLEVEL);
		row.GetColumn(num++, out this.MAXLEVEL);
		row.GetColumn(num++, out this.ReputeUnique);
		row.GetColumn(num++, out this.ReputeValue);
		row.GetColumn(num++, out this.DROPITEM);
	}
}
