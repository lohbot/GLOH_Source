using System;

namespace PROTOCOL.GAME
{
	public class GS_NEWGUILD_BOSS_ROOMINFO_ACK
	{
		public int i32Result;

		public short i16Floor;

		public byte byRoomState;

		public int i32BossHP;

		public byte byReward;

		public int i32Count;

		public long i64ClearPersonID;
	}
}
