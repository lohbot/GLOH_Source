using System;

namespace PROTOCOL.GAME
{
	public class GS_EXCHANGE_JEWELRY_REQ
	{
		public int nIDX;

		public int nType;

		public int nItemUnique;

		public int nItemNum;

		public long nItemID;

		public int nSelectNum;

		public GS_EXCHANGE_JEWELRY_REQ()
		{
			this.nIDX = 0;
			this.nType = 0;
			this.nItemUnique = 0;
			this.nItemNum = 0;
			this.nItemID = 0L;
			this.nSelectNum = 0;
		}
	}
}
