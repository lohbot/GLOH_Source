using System;
using System.Collections.Generic;

public class NEWEXPLORATION_RANK_REWARD_COMPARE : IComparer<NEWEXPLORATION_RANK_REWARD>
{
	public int Compare(NEWEXPLORATION_RANK_REWARD x, NEWEXPLORATION_RANK_REWARD y)
	{
		if (x.i16Index == y.i16Index)
		{
			return 0;
		}
		if (x.i16Index > y.i16Index)
		{
			return 1;
		}
		return -1;
	}
}
