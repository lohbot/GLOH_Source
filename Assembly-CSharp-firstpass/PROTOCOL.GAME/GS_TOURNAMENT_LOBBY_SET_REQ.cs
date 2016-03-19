using System;

namespace PROTOCOL.GAME
{
	public class GS_TOURNAMENT_LOBBY_SET_REQ
	{
		public int nLobbyIndex;

		public int eLobbyStep;

		public int[] nSoldierKind = new int[3];

		public byte[] nSolPos = new byte[3];
	}
}
