using System;
using System.Runtime.InteropServices;

namespace PROTOCOL.GAME
{
	[StructLayout(LayoutKind.Sequential)]
	public class GS_EXCHANGE_GUILDWAR_CHECK_ACK
	{
		public int nResult;

		public int nItemUnique;

		public int nExchangeLimit;

		public GS_EXCHANGE_GUILDWAR_CHECK_ACK()
		{
			this.nResult = 0;
			this.nItemUnique = 0;
			this.nExchangeLimit = 0;
		}
	}
}
