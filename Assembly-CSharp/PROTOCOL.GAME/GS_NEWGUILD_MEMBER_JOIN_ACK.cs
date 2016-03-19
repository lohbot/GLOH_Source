using System;

namespace PROTOCOL.GAME
{
	public class GS_NEWGUILD_MEMBER_JOIN_ACK
	{
		public int i32Result;

		public long i64PersonID;

		public long i64PersonID_NewMember;

		public char[] strName_NewMember = new char[11];

		public short i16Level;

		public bool bApprove;

		public char[] strGuildName = new char[11];

		public NEWGUILDMEMBER_INFO NewGuildMemberInfo = new NEWGUILDMEMBER_INFO();

		public long i64CurGuildID;
	}
}
