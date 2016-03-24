using System;
using System.Runtime.InteropServices;

namespace PROTOCOL.GAME
{
	[StructLayout(LayoutKind.Sequential)]
	public class GS_EXCHANGE_MYTHICSOL_ACK
	{
		public int nResult;

		public int nItemUnique;

		public int nItemNum;

		public GS_EXCHANGE_MYTHICSOL_ACK()
		{
			this.nResult = 0;
			this.nItemUnique = 0;
			this.nItemNum = 0;
		}
	}
}
