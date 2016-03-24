using System;
using System.Collections.Generic;
using TsLibs;

public class MYTHRAID_RANK_REWARD_INFO : NrTableData
{
	private readonly int MAX_REWARD_COUNT = 3;

	public int RANK;

	public List<int> REWARD_UNIQUE = new List<int>();

	public List<int> REWARD_NUMBER = new List<int>();

	public MYTHRAID_RANK_REWARD_INFO() : base(NrTableData.eResourceType.eRT_MYTHRAIDRANKREWARD)
	{
	}

	public override void SetData(TsDataReader.Row row)
	{
		int num = 0;
		row.GetColumn(num++, out this.RANK);
		for (int i = 0; i < this.MAX_REWARD_COUNT; i++)
		{
			int item;
			row.GetColumn(num++, out item);
			this.REWARD_UNIQUE.Add(item);
			int item2;
			row.GetColumn(num++, out item2);
			this.REWARD_NUMBER.Add(item2);
		}
	}
}
