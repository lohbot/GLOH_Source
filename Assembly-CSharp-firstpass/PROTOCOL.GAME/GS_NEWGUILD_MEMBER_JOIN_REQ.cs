using System;

namespace PROTOCOL.GAME
{
	public class GS_NEWGUILD_MEMBER_JOIN_REQ
	{
		public long i64PersonID_NewMember;

		public char[] strName_NewMember = new char[11];

		public short i16Level;

		public bool bApprove;
	}
}
