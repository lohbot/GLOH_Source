using System;

namespace PROTOCOL.GAME
{
	public class GS_NEWGUILD_LIST_ACK
	{
		public short i16CurPageNum;

		public short i16MaxPageNum;

		public short i16GuildListNum;

		public char[] strMasterName = new char[11];

		public char[] strGuildName = new char[11];

		public bool bIsGuildWarRank;
	}
}
