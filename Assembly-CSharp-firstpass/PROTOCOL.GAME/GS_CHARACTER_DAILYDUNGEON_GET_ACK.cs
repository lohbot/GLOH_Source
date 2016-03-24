using System;

namespace PROTOCOL.GAME
{
	public class GS_CHARACTER_DAILYDUNGEON_GET_ACK
	{
		public int i32Result;

		public int[] i32DayOfWeek = new int[5];

		public sbyte[] i8Diff = new sbyte[5];

		public int[] i32ResetCount = new int[5];

		public int[] i32IsClear = new int[5];

		public sbyte[] i8IsReward = new sbyte[5];
	}
}
