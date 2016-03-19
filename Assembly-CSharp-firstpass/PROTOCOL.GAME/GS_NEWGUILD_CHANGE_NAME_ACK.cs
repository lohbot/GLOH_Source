using System;

namespace PROTOCOL.GAME
{
	public class GS_NEWGUILD_CHANGE_NAME_ACK
	{
		public int i32Result;

		public char[] strGuildName = new char[11];

		public long i64PersonID;
	}
}
