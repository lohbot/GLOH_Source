using System;

namespace PROTOCOL.GAME
{
	public class GS_TOURNAMENT_MATCHLIST_INFO
	{
		public int nIndex;

		public char[] szCharName1 = new char[21];

		public char[] szCharName2 = new char[21];

		public char[] szObserver = new char[21];

		public int nPlayerState1;

		public int nPlayerState2;

		public int i32FirstTurn;

		public int nWinCount1;

		public int nWinCount2;

		public bool bUseLobby;
	}
}
