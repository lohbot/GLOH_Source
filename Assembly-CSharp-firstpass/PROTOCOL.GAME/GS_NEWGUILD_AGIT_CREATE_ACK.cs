using System;

namespace PROTOCOL.GAME
{
	public class GS_NEWGUILD_AGIT_CREATE_ACK
	{
		public long i64GuildID;

		public short i16AgitLevel;

		public long i64AgitExp;

		public long i64CurGuildFund;

		public long i64NeedGuildFund;

		public long i64PersonID;

		public int i32Result;

		public long i64AfterGuildFund;
	}
}