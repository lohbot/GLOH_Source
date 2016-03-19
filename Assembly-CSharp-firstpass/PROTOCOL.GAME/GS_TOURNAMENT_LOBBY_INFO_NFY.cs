using System;

namespace PROTOCOL.GAME
{
	public class GS_TOURNAMENT_LOBBY_INFO_NFY
	{
		public char[] szPlayer1 = new char[21];

		public char[] szPlayer2 = new char[21];

		public int nLobbyIndex;

		public int nPlayerUnique1;

		public int nPlayerUnique2;

		public int nBatchKindTotal;

		public int nSoldierCount;
	}
}
