using System;

namespace PROTOCOL.GAME
{
	public class GS_INFIBATTLE_START_REQ
	{
		public byte i8PracticeBattleMode;

		public long[] i64SolID = new long[15];

		public long[] i64BattlePos = new long[15];
	}
}
