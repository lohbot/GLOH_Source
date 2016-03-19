using System;

namespace PROTOCOL.GAME
{
	public class GS_SOLDIER_CHANGE_BATTLEPOS_ACK
	{
		public long[] SolID = new long[6];

		public int[] BattlePos = new int[6];
	}
}
