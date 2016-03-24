using System;

namespace PROTOCOL
{
	public class GUILDWAR_RANK_DB
	{
		public long i64GuildID;

		public int i32Rank;

		public long i64LastWeekEnemyGuildID;

		public int i32LastWeekGuildPoint;

		public bool ui8IsLastWeekWin;

		public bool ui8IsGetReward;
	}
}
