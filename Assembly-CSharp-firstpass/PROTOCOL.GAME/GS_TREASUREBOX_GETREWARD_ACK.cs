using System;

namespace PROTOCOL.GAME
{
	public class GS_TREASUREBOX_GETREWARD_ACK
	{
		public short i16TreasureUnique;

		public int i32Result;

		public long i64Money;

		public int[] i32ItemUnique = new int[5];

		public int[] i32ItemNum = new int[5];
	}
}
