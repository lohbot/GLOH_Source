using GAME;
using System;

namespace PROTOCOL.GAME
{
	public class GS_CHAR_MOVE_REQ
	{
		public POS3D PosStart = new POS3D();

		public POS3D PosDest = new POS3D();

		public POS3D Direction = new POS3D();
	}
}
