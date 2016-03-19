using System;

namespace PROTOCOL.GAME
{
	public class GS_ITEM_SELL_MULTI_ACK
	{
		public int i32Result = -1;

		public long[] i64ItemID = new long[30];

		public long i64CurTotalSellMoney;

		public long i64AddMoney;

		public long i64AfterMoney;

		public GS_ITEM_SELL_MULTI_ACK()
		{
			for (int i = 0; i < 30; i++)
			{
				this.i64ItemID[i] = 0L;
			}
		}
	}
}
