using System;

namespace PROTOCOL.GAME
{
	public class GS_GUILDWAR_RAIDBATTLEPOS_CHANGE_REQ
	{
		public byte ui8RaidUnique;

		public long[] i64MilitaryID = new long[9];

		public byte[] ui8RaidBattlePos = new byte[9];
	}
}
