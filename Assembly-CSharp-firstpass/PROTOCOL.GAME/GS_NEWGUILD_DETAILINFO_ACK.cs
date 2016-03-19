using System;

namespace PROTOCOL.GAME
{
	public class GS_NEWGUILD_DETAILINFO_ACK
	{
		public long i64GuildID;

		public char[] strGuildName = new char[11];

		public char[] strMasterName = new char[11];

		public char[] strSubMasterName = new char[11];

		public short i16CurMemberNum;

		public short i16MaxMemberNum;

		public short i16Rank;

		public int i32Point;

		public short i16Level;

		public long i64Exp;

		public char[] strGuildMessage = new char[50];

		public short i16ApplicantNum;

		public long i64CreateDate;

		public long i64Fund;
	}
}
