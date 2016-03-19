using System;

namespace PROTOCOL.GAME
{
	public class GS_SOLDIERS_SELL_ACK
	{
		public int i32Result;

		public byte i8Cnt;

		public long[] i64SolID = new long[50];
	}
}
