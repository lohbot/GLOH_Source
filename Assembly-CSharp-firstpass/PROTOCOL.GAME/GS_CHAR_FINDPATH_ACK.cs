using GAME;
using System;

namespace PROTOCOL.GAME
{
	public class GS_CHAR_FINDPATH_ACK
	{
		public short CharUnique;

		public int PathCount;

		public POS3D CurPos = new POS3D();
	}
}
