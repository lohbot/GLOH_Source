using System;

namespace PROTOCOL.GAME
{
	public class GS_NEWGUILD_MEMBER_LEAVE_ACK
	{
		public int i32Result;

		public long i64PersonID;

		public char[] strCharName = new char[11];
	}
}
