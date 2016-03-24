using System;

namespace PROTOCOL.GAME
{
	public class GS_NEWEXPLORATION_TREASURE_ACK
	{
		public int i32Result;

		public int i32WeekDataType;

		public long i64WeekDataValue;

		public int[] i32ItemUnique = new int[3];

		public int[] i32ItemNum = new int[3];
	}
}
