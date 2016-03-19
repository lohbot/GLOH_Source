using System;

namespace PROTOCOL.GAME
{
	public class GS_AUCTION_DIRECTPURCHASE_ACK
	{
		public int i32Result = -1;

		public long i64AuctionID;

		public long i64DirectCostMoney;

		public int i32DirectCostHearts;

		public long i64CurCostMoney;

		public int i32CurHeartsNum;

		public int i32CharDetailType;

		public long i64CharDetailValue;
	}
}
