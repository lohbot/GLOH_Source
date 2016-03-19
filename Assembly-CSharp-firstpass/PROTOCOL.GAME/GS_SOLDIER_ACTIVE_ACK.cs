using System;

namespace PROTOCOL.GAME
{
	public struct GS_SOLDIER_ACTIVE_ACK
	{
		public long ActiveSolID;

		public int Index;

		public long ReadySolID;

		public int BattlePos;
	}
}
