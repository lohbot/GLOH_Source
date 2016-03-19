using GAME;
using System;

namespace PROTOCOL.GAME
{
	public class GS_CHAR_MOVE_ACK
	{
		public byte MoveType;

		public short CharUnique;

		public POS3D PosStart = new POS3D();

		public POS3D PosDest = new POS3D();

		public float Speed;
	}
}
