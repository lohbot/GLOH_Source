using System;

namespace PROTOCOL.GAME
{
	public class GS_NEWGUILD_MEMBER_FORCE_APPLY_REQ
	{
		public long i64BeforeGuildID;

		public long i64GuildID;

		public char[] strCharName = new char[11];
	}
}
