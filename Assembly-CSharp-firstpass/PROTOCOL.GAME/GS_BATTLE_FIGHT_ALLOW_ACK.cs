using System;

namespace PROTOCOL.GAME
{
	public class GS_BATTLE_FIGHT_ALLOW_ACK
	{
		public char[] szCharName = new char[21];

		public short nCharUnique;

		public long nPersonID;

		public int nLevel;
	}
}
