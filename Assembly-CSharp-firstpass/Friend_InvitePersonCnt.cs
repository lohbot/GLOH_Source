using System;
using TsLibs;

public class Friend_InvitePersonCnt : NrTableData
{
	public int CHECK_LEVEL;

	public int CANINVITE_PERSONCOUNT;

	public Friend_InvitePersonCnt()
	{
		this.Init();
	}

	public void Init()
	{
		this.CHECK_LEVEL = 0;
		this.CANINVITE_PERSONCOUNT = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.CHECK_LEVEL);
		row.GetColumn(num++, out this.CANINVITE_PERSONCOUNT);
	}
}
