using System;

namespace PROTOCOL.GAME
{
	public class GS_NEWGUILD_INVITE_ACK
	{
		public int i32Result;

		public long i64PersonID;

		public char[] strInviteCharName = new char[11];

		public char[] strGuildName = new char[11];
	}
}
