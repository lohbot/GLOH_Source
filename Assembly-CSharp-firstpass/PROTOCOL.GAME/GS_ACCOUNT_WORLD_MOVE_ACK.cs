using System;

namespace PROTOCOL.GAME
{
	public class GS_ACCOUNT_WORLD_MOVE_ACK
	{
		public int Result;

		public long m_nUID;

		public long i64MovingWorld_KEY;

		public char[] szMoveFrontServerIP = new char[16];

		public short i16Port;

		public long m_nCHMoveTargetPersonID;

		public byte m_nCHMoveType;

		public byte i8AgitMove;
	}
}
