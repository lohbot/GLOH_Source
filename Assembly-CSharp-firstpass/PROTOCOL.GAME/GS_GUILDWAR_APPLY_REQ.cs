using System;

namespace PROTOCOL.GAME
{
	public class GS_GUILDWAR_APPLY_REQ
	{
		public byte ui8RaidUnique;

		public byte ui8RaidBattlePos;

		public long[] i64SolID = new long[5];

		public byte[] ui8SolPosIndex = new byte[5];

		public short[] i16BattlePos = new short[5];
	}
}
