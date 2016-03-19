using System;

namespace PROTOCOL.GAME
{
	public class GS_EXPEDITION_START_ACK
	{
		public int i32Result;

		public byte ui8ExpeditionMilitaryUniq;

		public long[] i64SolID = new long[15];

		public byte[] i8SolPosIndex = new byte[15];

		public short[] i16BattlePos = new short[15];

		public long i64TotalCount_ExpeditionDayjoin;
	}
}
