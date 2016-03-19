using System;

namespace PROTOCOL.GAME
{
	public class GS_SOLDIER_GROUPSTATUS_UPDATE_NFY
	{
		public long[] SolID = new long[6];

		public byte[] SolStatus = new byte[6];

		public short[] BattlePos = new short[6];
	}
}
