using System;

namespace PROTOCOL.GAME
{
	public class GS_EXPEDITION_START_REQ
	{
		public byte ui8ExpeditionGrade;

		public long[] i64SolID = new long[15];

		public byte[] i8PosIndex = new byte[15];

		public short[] i16BattlePos = new short[15];
	}
}
