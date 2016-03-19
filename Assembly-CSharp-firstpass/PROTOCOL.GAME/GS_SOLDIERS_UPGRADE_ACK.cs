using System;

namespace PROTOCOL.GAME
{
	public class GS_SOLDIERS_UPGRADE_ACK
	{
		public int i32Result;

		public long i64BaseSolID;

		public byte i8Cnt;

		public long[] i64SubSolID = new long[50];

		public short i16Level;

		public byte ui8Grade;

		public long i64Exp;

		public long i64AddExp;

		public long nEvolutionExp;

		public long nAddEvolutionExp;

		public int i32TradeCount;

		public long nMaxLevelEvolution;
	}
}
