using System;

namespace PROTOCOL.GAME
{
	public class GS_NEWGUILD_AGIT_ADD_NPC_ACK
	{
		public long i64GuildID;

		public byte ui8NPCType;

		public short i16NPCLevel;

		public long i64NPCEndTime;

		public long i64PersonID;

		public int i32Result;

		public long i64AfterGuildFund;
	}
}
