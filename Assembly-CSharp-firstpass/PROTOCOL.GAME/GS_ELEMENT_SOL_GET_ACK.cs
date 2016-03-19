using System;

namespace PROTOCOL.GAME
{
	public class GS_ELEMENT_SOL_GET_ACK
	{
		public int i32Result;

		public long[] i64SolID = new long[5];

		public long i64CurrentMoney;

		public int SolCount;

		public int SolSubDataCount;
	}
}
