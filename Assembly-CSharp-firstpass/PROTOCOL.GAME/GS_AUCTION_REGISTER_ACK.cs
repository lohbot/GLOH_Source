using System;

namespace PROTOCOL.GAME
{
	public class GS_AUCTION_REGISTER_ACK
	{
		public int i32Result = -1;

		public short i16PurchaseMaxPageNum;

		public int i32ItemUnique;

		public long i64SolID;

		public long i64Money;

		public int i32CharDetailType;

		public long i64CharDetailValue;
	}
}
