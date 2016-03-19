using System;

namespace PROTOCOL.GAME
{
	public class GS_BATTLE_CLOSE_NFY
	{
		public int iBFRoomID;

		public int iCloseReason;

		public int[] iValue = new int[3];

		public short iCharUnique;

		public byte i8BattleState;
	}
}
