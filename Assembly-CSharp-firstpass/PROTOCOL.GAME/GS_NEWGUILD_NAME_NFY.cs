using System;

namespace PROTOCOL.GAME
{
	public class GS_NEWGUILD_NAME_NFY
	{
		public long PersonID;

		public long i64GuildID;

		public bool bGuildPortrait;

		public char[] szGuildName = new char[11];

		public bool bGuildWar;
	}
}
