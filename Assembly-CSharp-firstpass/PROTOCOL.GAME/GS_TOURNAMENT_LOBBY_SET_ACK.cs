using System;

namespace PROTOCOL.GAME
{
	public class GS_TOURNAMENT_LOBBY_SET_ACK
	{
		public int eLobbyStep;

		public byte nActiveAlly;

		public int[] nSoldierKind = new int[3];

		public byte[] nSolPos = new byte[3];
	}
}
