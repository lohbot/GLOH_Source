using System;

namespace PROTOCOL.GAME
{
	public class GS_ITEM_SELL_MULTI_REQ
	{
		public long[] i64ItemID = new long[30];

		public GS_ITEM_SELL_MULTI_REQ()
		{
			for (int i = 0; i < 30; i++)
			{
				this.i64ItemID[i] = 0L;
			}
		}
	}
}
