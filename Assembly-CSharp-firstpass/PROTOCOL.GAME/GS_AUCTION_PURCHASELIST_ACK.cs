using System;

namespace PROTOCOL.GAME
{
	public class GS_AUCTION_PURCHASELIST_ACK
	{
		public int i32AuctionHeartsUse;

		public int i32AuctionGoldUse;

		public float fAuctionCommission;

		public int i32AuctionSellMaxNum;

		public long lAuctionSellPrice;

		public long lAuctionDuration;

		public long lAuctionDurationExtend;

		public int i32AuctionTenderMaxNum;

		public int i32AuctionTenderShowNum;

		public float fAuctionTenderRate;

		public float fAuctionSellPriceRate;

		public int i32HeartsValue;

		public int i32DailySellLimit;

		public int i32DailyBuyLimit;

		public short i16SolLevelLimit;

		public short i16SolSkillLevelLimit;

		public short i16CurPageNum;

		public short i16MaxPageNum;

		public byte i8RegisterType;

		public short i16ItemNum;

		public short i16SolNum;

		public bool bSearchButton;
	}
}
