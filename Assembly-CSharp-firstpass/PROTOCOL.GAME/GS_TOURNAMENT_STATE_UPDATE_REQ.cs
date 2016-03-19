using System;

namespace PROTOCOL.GAME
{
	public class GS_TOURNAMENT_STATE_UPDATE_REQ
	{
		public char[] szPlayerName = new char[21];

		public int nMatchState;
	}
}
