using System;

namespace PROTOCOL
{
	public class NEWGUILD_INFO
	{
		public long i64GuildID;

		public short i16Level;

		public long i64Exp;

		public char[] strGuildName = new char[11];

		public long i64CreateDate;

		public byte ui8SetImage;

		public char[] strGuildMessage = new char[50];

		public int i32Point;

		public short i16Rank;

		public long i64Fund;

		public char[] strGuildNotice = new char[50];

		public int i32GuildWarPoint;

		public bool bIsExitAgit;

		public bool bIsGuildWar;

		public bool bIsGuildWarCancelReservation;
	}
}
