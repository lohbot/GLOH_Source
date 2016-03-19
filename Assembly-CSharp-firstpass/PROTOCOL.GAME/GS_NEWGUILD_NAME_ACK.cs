using System;

namespace PROTOCOL.GAME
{
	public class GS_NEWGUILD_NAME_ACK
	{
		public long i64PersonID;

		public long i64GuildID;

		public byte ui8SetImage;

		public char[] strGuildName = new char[11];
	}
}
