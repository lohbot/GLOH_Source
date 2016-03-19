using System;

namespace PROTOCOL.GAME
{
	public class GS_SOLDIER_CHANGE_BATTLEPOS_REQ
	{
		public byte MilitaryUnique;

		public long[] SolID = new long[6];

		public int[] BattlePos = new int[6];
	}
}
