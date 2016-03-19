using GAME;
using System;

namespace PROTOCOL.GAME
{
	public class NPC_POSITION
	{
		public short i16CharUnique;

		public short i16CharCode;

		public POS3D Pos = new POS3D();

		public char[] strName = new char[21];
	}
}
