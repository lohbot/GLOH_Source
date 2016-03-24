using System;

namespace PROTOCOL.GAME
{
	public class GS_CURRENT_CASHINFO_ACK
	{
		public int nType;

		public long nRealHearts;

		public long nFreeHearts;

		public char[] szServiceUrl = new char[128];
	}
}
