using SERVICE;
using System;

namespace PROTOCOL.GAME
{
	public class GS_CLIENT_STANDBY_REQ
	{
		public byte Mode;

		public byte LowMemory;

		public USER_CLIENT_INFO kClientInfo = new USER_CLIENT_INFO();
	}
}
