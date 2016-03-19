using System;

namespace PROTOCOL.GAME
{
	public class GS_POINT_BUY_ACK
	{
		public int nResult;

		public int nAddPointType;

		public int nType;

		public long nPoint;

		public int nIndex = -1;

		public long nValue = -1L;

		public GS_POINT_BUY_ACK()
		{
			this.nResult = 0;
			this.nType = 0;
			this.nPoint = 0L;
			this.nIndex = -1;
			this.nValue = -1L;
		}
	}
}
