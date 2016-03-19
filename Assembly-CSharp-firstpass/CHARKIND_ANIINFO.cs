using System;
using TsLibs;

public class CHARKIND_ANIINFO : NrTableData
{
	public string BUNDLENAME = string.Empty;

	public string WEAPONTYPE = string.Empty;

	public string ANITYPE = string.Empty;

	public string EVENTTYPE = string.Empty;

	public float EVENTTIME;

	public CHARKIND_ANIINFO() : base(NrTableData.eResourceType.eRT_CHARKIND_ANIINFO)
	{
	}

	public override void SetData(TsDataReader.Row row)
	{
		int num = 0;
		row.GetColumn(num++, out this.BUNDLENAME);
		row.GetColumn(num++, out this.WEAPONTYPE);
		row.GetColumn(num++, out this.ANITYPE);
		row.GetColumn(num++, out this.EVENTTYPE);
		row.GetColumn(num++, out this.EVENTTIME);
	}
}
