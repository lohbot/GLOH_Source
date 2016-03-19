using System;

namespace GAME
{
	public class CHAR_MOVE_INFO
	{
		public short CharUnique;

		public byte MoveType;

		public POS3D PosStart = new POS3D();

		public POS3D PosDest = new POS3D();

		public float Speed;
	}
}
