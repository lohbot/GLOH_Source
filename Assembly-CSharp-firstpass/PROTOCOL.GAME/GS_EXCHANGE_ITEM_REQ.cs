using System;

namespace PROTOCOL.GAME
{
	public class GS_EXCHANGE_ITEM_REQ
	{
		public int nItemUnique;

		public long nItemNum;

		public GS_EXCHANGE_ITEM_REQ()
		{
			this.nItemUnique = 0;
			this.nItemNum = 0L;
		}
	}
}
