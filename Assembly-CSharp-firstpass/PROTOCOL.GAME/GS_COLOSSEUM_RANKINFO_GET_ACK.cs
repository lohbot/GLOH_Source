using System;

namespace PROTOCOL.GAME
{
	public class GS_COLOSSEUM_RANKINFO_GET_ACK
	{
		public int i32Result;

		public byte byRank_GetType;

		public long[] TopGuildRank = new long[3];

		public int i32Page;

		public int count;
	}
}
