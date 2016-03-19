using System;

namespace PROTOCOL.GAME
{
	public class GS_TOURNAMENT_MATCH_UPDATE_REQ
	{
		public int nIndex;

		public char[] szCharName1 = new char[21];

		public char[] szCharName2 = new char[21];

		public char[] szObserver = new char[21];

		public int nPlayerState1;

		public int nPlayerState2;

		public int i32FirstTurn;

		public bool bDelete;

		public bool bUseLobby;
	}
}
