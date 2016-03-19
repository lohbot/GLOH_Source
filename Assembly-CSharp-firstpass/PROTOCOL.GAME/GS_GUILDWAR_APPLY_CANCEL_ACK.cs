using System;

namespace PROTOCOL.GAME
{
	public class GS_GUILDWAR_APPLY_CANCEL_ACK
	{
		public byte ui8RaidUnique;

		public byte ui8MilitaryUnique;

		public int i32Result = -1;

		public long[] i64SolID = new long[5];
	}
}
