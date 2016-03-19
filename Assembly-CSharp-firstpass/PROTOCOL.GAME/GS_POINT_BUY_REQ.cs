using System;

namespace PROTOCOL.GAME
{
	public class GS_POINT_BUY_REQ
	{
		public int nAddPointType;

		public long nPoint;

		public int nType;

		public long[] nItemID = new long[10];

		public int nItemUnique;

		public long nItemNum;

		public GS_POINT_BUY_REQ()
		{
			this.nType = 0;
			for (int i = 0; i < 10; i++)
			{
				this.nItemID[i] = 0L;
			}
			this.nItemUnique = 0;
			this.nItemNum = 0L;
		}
	}
}
