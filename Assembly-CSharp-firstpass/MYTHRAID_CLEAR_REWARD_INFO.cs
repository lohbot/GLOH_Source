using System;
using TsLibs;

public class MYTHRAID_CLEAR_REWARD_INFO : NrTableData
{
	public static readonly int MAX_REWARD_COUNT = 3;

	public int ROUND;

	public int CLEARMODE;

	public int[] REWARD_UNIQUE = new int[MYTHRAID_CLEAR_REWARD_INFO.MAX_REWARD_COUNT];

	public int[] REWARD_NUMBER = new int[MYTHRAID_CLEAR_REWARD_INFO.MAX_REWARD_COUNT];

	public MYTHRAID_CLEAR_REWARD_INFO() : base(NrTableData.eResourceType.eRT_MYTHRAIDCLEARREWARD)
	{
	}

	public override void SetData(TsDataReader.Row row)
	{
		int num = 0;
		row.GetColumn(num++, out this.ROUND);
		row.GetColumn(num++, out this.CLEARMODE);
		for (int i = 0; i < MYTHRAID_CLEAR_REWARD_INFO.MAX_REWARD_COUNT; i++)
		{
			int num2;
			row.GetColumn(num++, out num2);
			this.REWARD_UNIQUE[i] = num2;
			int num3;
			row.GetColumn(num++, out num3);
			this.REWARD_NUMBER[i] = num3;
		}
	}

	public static int setDataKey(int mode, int clearRound)
	{
		return mode * 100 + clearRound;
	}
}
