using System;

namespace PROTOCOL.GAME
{
	public class AUCTION_TABLE_INFO
	{
		public byte i8Type;

		public int i32Unique;

		public long i64MinHearts;

		public long i64MaxHearts;

		public long i64MinGold;

		public long i64MaxGold;

		public long i64TradeLimitHearts;

		public long i64TradeLimitGold;

		public short[] i16SkillLevel = new short[3];

		public long[] i64NextTradeLimitHearts = new long[3];

		public long[] i64NextTradeLimitGold = new long[3];
	}
}
