using System;

namespace PROTOCOL.GAME
{
	public class GS_EXCHANGE_JEWELRY_ACK
	{
		public int nResult;

		public int nItemUnique;

		public int nItemNum;

		public GS_EXCHANGE_JEWELRY_ACK()
		{
			this.nResult = 0;
			this.nItemUnique = 0;
			this.nItemNum = 0;
		}
	}
}
