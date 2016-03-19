using System;

namespace PROTOCOL.GAME
{
	public class GS_WARP_ACK
	{
		public int m_byResult;

		public byte Mode;

		public int MapUnique;

		public uint CheckSum;

		public byte BattleMapIdx;

		public int nIndunUnique = -1;
	}
}
