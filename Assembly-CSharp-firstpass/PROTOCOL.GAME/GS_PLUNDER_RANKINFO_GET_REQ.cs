using System;

namespace PROTOCOL.GAME
{
	public class GS_PLUNDER_RANKINFO_GET_REQ
	{
		public byte ui8Rank_GetType;

		public char[] szSearchName = new char[21];
	}
}
