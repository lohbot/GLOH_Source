using System;

namespace PROTOCOL.GAME
{
	public class GS_GUILDWAR_INFO_ACK
	{
		public long[] i64GuildID = new long[2];

		public int[] i32Rank = new int[2];

		public short[] i16Level = new short[2];

		public short[] i16CurMemberNum = new short[2];

		public short[] i16MaxMemberNum = new short[2];

		public int[] i32WarPoint = new int[2];

		public int[] i32WarMatchPoint = new int[2];

		public long[] i64Exp = new long[2];

		public long[] i64BirthDate = new long[2];

		public char[][] strGuildName = new char[2][];

		public byte bGuildWarTimeState;

		public bool bIsGuildWar;

		public bool bIsCancelReservation;

		public GS_GUILDWAR_INFO_ACK()
		{
			this.strGuildName[0] = new char[11];
			this.strGuildName[1] = new char[11];
		}
	}
}
