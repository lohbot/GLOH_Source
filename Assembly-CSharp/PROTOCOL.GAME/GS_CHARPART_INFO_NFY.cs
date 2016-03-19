using GAME;
using System;

namespace PROTOCOL.GAME
{
	public class GS_CHARPART_INFO_NFY
	{
		public short CharUnique;

		public int i32CharKind;

		public CHAR_SHAPE_INFO kShapeInfo = new CHAR_SHAPE_INFO();
	}
}
