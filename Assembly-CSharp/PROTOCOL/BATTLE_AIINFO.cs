using System;

namespace PROTOCOL
{
	public class BATTLE_AIINFO
	{
		public short i16MyBUID;

		public char[] MyCharName = new char[21];

		public short MyPos_x;

		public short MyPos_y;

		public byte i8MyCharTurn;

		public short i16TargetBUID;

		public char[] TargetCharName = new char[21];

		public short TargetPos_x;

		public short TargetPos_y;

		public int i32TargetAgro;
	}
}
