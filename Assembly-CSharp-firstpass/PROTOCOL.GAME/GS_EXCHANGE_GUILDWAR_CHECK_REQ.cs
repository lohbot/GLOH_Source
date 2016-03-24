using System;
using System.Runtime.InteropServices;

namespace PROTOCOL.GAME
{
	[StructLayout(LayoutKind.Sequential)]
	public class GS_EXCHANGE_GUILDWAR_CHECK_REQ
	{
		public int nItemUnique;

		public int nItemNum;

		public long nItemID;

		public GS_EXCHANGE_GUILDWAR_CHECK_REQ()
		{
			this.nItemUnique = 0;
			this.nItemNum = 0;
			this.nItemID = 0L;
		}
	}
}
