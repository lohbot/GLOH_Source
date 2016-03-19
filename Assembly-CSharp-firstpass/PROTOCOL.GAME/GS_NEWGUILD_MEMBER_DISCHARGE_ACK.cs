using System;

namespace PROTOCOL.GAME
{
	public class GS_NEWGUILD_MEMBER_DISCHARGE_ACK
	{
		public int i32Result;

		public long i64DischargePersonID;

		public char[] strCharName = new char[11];

		public char[] strGuildName = new char[11];
	}
}
