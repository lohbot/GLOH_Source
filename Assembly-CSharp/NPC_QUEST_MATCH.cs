using System;
using TsLibs;

public class NPC_QUEST_MATCH
{
	public long i64MatchUnique;

	public string CharCode = string.Empty;

	public int NNPCID;

	public int NQUESTGROUPID;

	public string strQUESTID = string.Empty;

	public void SetData(TsDataReader.Row row)
	{
		this.i64MatchUnique = 0L;
		this.CharCode = string.Empty;
		this.NQUESTGROUPID = 0;
		this.strQUESTID = string.Empty;
		int num = 0;
		row.GetColumn(num++, out this.i64MatchUnique);
		row.GetColumn(num++, out this.CharCode);
		row.GetColumn(num++, out this.NQUESTGROUPID);
		row.GetColumn(num++, out this.strQUESTID);
	}
}
