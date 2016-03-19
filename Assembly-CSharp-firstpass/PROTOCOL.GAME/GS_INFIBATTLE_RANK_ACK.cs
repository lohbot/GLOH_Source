using System;

namespace PROTOCOL.GAME
{
	public class GS_INFIBATTLE_RANK_ACK
	{
		public byte ui8Type;

		public long i64PersonID;

		public int i32StartRank;

		public int i32RankCount;

		public int i32MaxRankCount;

		public int i32SubDataCount;
	}
}
