using System;

namespace PROTOCOL.GAME
{
	public class GS_AUCTION_PURCHASELIST_REQ
	{
		public short i16PageNum;

		public byte i8RegisterType;

		public byte i8ItemType;

		public short i16UseMinLevel;

		public short i16UseMaxLevel;

		public int i32ItemSkillUnique;

		public short i16ItemSkillLevel;

		public short i16ItemTradeCount;

		public byte i8SolSeason;

		public short i16SolLevel;

		public char[] strSolName = new char[32];

		public short SolTradeCount;

		public long i64CostMoney;

		public long i64DirectCostMoney;

		public int i32CostHearts;

		public int i32DirectCostHearts;

		public byte i8PayType;

		public byte i8SortType;

		public bool bSearchButton;
	}
}
