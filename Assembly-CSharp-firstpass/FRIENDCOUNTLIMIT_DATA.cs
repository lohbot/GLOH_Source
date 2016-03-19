using System;
using TsLibs;

public class FRIENDCOUNTLIMIT_DATA : NrTableData
{
	public short Level_Min;

	public short Level_Max;

	public int FriendLimitCount;

	public FRIENDCOUNTLIMIT_DATA()
	{
		this.Init();
	}

	public void Init()
	{
		this.Level_Min = 0;
		this.Level_Max = 0;
		this.FriendLimitCount = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.Level_Min);
		row.GetColumn(num++, out this.Level_Max);
		row.GetColumn(num++, out this.FriendLimitCount);
	}
}
