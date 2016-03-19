using System;

namespace PROTOCOL.GAME
{
	public class GS_AUCTION_TENDER_ACK
	{
		public int i32Result;

		public long i64AuctionID;

		public long i64CostMoney;

		public int i32CostHearts;

		public long i64CurCostMoney;

		public int i32CurHeartsNum;

		public int i32ServerResult;
	}
}
