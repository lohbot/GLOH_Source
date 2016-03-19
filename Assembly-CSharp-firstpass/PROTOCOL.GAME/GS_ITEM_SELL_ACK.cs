using System;

namespace PROTOCOL.GAME
{
	public class GS_ITEM_SELL_ACK
	{
		public int i32Result = -1;

		public long i64ItemID;

		public int i32ItemUnique;

		public long i64TotalSellMoney;

		public long i64AddMoney;

		public long i64AfterMoney;
	}
}
