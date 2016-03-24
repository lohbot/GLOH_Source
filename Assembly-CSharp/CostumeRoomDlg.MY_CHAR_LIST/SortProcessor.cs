using System;
using System.Collections.Generic;

namespace CostumeRoomDlg.MY_CHAR_LIST
{
	public class SortProcessor
	{
		public void SortMySolListByPower(ref List<NkSoldierInfo> costumeKindMySolList)
		{
			if (costumeKindMySolList == null || costumeKindMySolList.Count == 0)
			{
				return;
			}
			costumeKindMySolList.Sort(new Comparison<NkSoldierInfo>(this.CompareSolPowerAscend));
		}

		private int CompareSolPowerAscend(NkSoldierInfo a, NkSoldierInfo b)
		{
			if (a.GetFightPower() < b.GetFightPower())
			{
				return 1;
			}
			if (a.GetFightPower() != b.GetFightPower())
			{
				return -1;
			}
			if (a.GetLevel() < b.GetLevel())
			{
				return 1;
			}
			if (a.GetLevel() > b.GetLevel())
			{
				return -1;
			}
			return 0;
		}
	}
}
