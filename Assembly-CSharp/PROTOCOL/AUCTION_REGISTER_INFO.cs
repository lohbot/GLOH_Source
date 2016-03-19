using System;

namespace PROTOCOL
{
	public class AUCTION_REGISTER_INFO
	{
		public long i64AuctionID;

		public long i64PersonID;

		public long i64ItemID;

		public long i64SolID;

		public long i64CostMoney;

		public long i64DirectCostMoney;

		public int i32CostHearts;

		public int i32DirectCostHearts;

		public long tmRegisterTime;

		public long tmExpireTime;

		public long i64TenderPersonID;

		public byte i8State;
	}
}
