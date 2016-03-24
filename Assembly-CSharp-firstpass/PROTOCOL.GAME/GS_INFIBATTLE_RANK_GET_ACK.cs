using System;

namespace PROTOCOL.GAME
{
	public class GS_INFIBATTLE_RANK_GET_ACK
	{
		public long i64PersonID;

		public int i32InfinityBattle_Rank;

		public int i32InfinityBattle_OldRank;

		public int i32InfiBattleStraightWin;

		public byte i8GetReward;

		public int i32InfinityBattle_TotalCount;

		public int i32InfinityBattle_WinCount;

		public int i32Result;
	}
}
