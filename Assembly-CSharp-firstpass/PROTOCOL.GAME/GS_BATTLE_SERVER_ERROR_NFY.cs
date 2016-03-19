using GAME;
using System;

namespace PROTOCOL.GAME
{
	public class GS_BATTLE_SERVER_ERROR_NFY
	{
		public short i16BUID;

		public int i32ServerErrorType;

		public byte i8OrderType;

		public POS3D Pos = new POS3D();
	}
}
