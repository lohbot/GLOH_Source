using System;

namespace PROTOCOL.GAME
{
	public class GS_SERVER_CHARINFO_ACK
	{
		public short i16MonsterCount;

		public short i16NPCCount;

		public short i16UserCount;

		public short i16MaxUserCount;

		public short i16CurUserCount;

		public long i64TotalTime;

		public long i64RealTime;

		public bool bTimeOnly;
	}
}
