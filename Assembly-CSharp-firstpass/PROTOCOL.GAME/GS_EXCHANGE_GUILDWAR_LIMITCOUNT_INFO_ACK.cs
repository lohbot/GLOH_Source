using System;
using System.Runtime.InteropServices;

namespace PROTOCOL.GAME
{
	[StructLayout(LayoutKind.Sequential)]
	public class GS_EXCHANGE_GUILDWAR_LIMITCOUNT_INFO_ACK
	{
		public long nPersonID;

		public int[] nItemUnique = new int[30];

		public int[] nExchangeLimit = new int[30];

		public GS_EXCHANGE_GUILDWAR_LIMITCOUNT_INFO_ACK()
		{
			this.nPersonID = 0L;
			for (int i = 0; i < 30; i++)
			{
				this.nItemUnique[i] = 0;
				this.nExchangeLimit[i] = 0;
			}
		}
	}
}
