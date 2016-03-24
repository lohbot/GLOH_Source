using System;
using TsLibs;

public class MYTHRAID_GUARDIANANGEL_INFO : NrTableData
{
	public static readonly int MAX_REWARD_COUNT = 3;

	public int UNIQUE;

	public string IMAGE = string.Empty;

	public int SKILLUNIQUE;

	public string EFFECTKEY = string.Empty;

	public string MOOVIEKEY = string.Empty;

	public MYTHRAID_GUARDIANANGEL_INFO() : base(NrTableData.eResourceType.eRT_MYTHRAIDGUARDIANANGEL)
	{
	}

	public override void SetData(TsDataReader.Row row)
	{
		int num = 0;
		row.GetColumn(num++, out this.UNIQUE);
		row.GetColumn(num++, out this.IMAGE);
		row.GetColumn(num++, out this.SKILLUNIQUE);
		row.GetColumn(num++, out this.EFFECTKEY);
		row.GetColumn(num++, out this.MOOVIEKEY);
	}
}
