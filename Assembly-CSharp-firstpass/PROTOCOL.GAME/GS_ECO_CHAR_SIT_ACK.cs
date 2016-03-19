using GAME;
using System;

namespace PROTOCOL.GAME
{
	public class GS_ECO_CHAR_SIT_ACK
	{
		public short i16CharUnique;

		public POS3D PosSit = new POS3D();

		public POS3D PosLookAt = new POS3D();
	}
}
