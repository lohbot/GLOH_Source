using System;

namespace GAME
{
	public class NEW_MAKECHAR_INFO
	{
		public short CharUnique;

		public long PersonID;

		public char[] CharName = new char[21];

		public short Level;

		public POS3D CharPos = new POS3D();

		public POS3D Direction = new POS3D();

		public float Speed;

		public long Status;

		public long SolID;

		public int CharKind;

		public byte CharKindType;
	}
}
