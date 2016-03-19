using System;

namespace PROTOCOL.GAME
{
	public class GS_NEWGUILD_MEMBER_APPLY_ACK
	{
		public int i32Result;

		public long i64GuildID;

		public char[] strGuildName = new char[11];

		public NEWGUILDMEMBER_APPLICANT_INFO ApplicantInfo = new NEWGUILDMEMBER_APPLICANT_INFO();
	}
}
