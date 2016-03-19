using System;

namespace PROTOCOL
{
	public class SCENARIO_LOBBY_INFO
	{
		public short i16Lobby_idx;

		public int i32Scenario_ID;

		public byte i8Difficulty;

		public byte i8User_max;

		public byte i8User_count;

		public byte i8Status;

		public short i16LimitTime;

		public long i64StartTime;

		public char[] szOwner_name = new char[21];
	}
}
