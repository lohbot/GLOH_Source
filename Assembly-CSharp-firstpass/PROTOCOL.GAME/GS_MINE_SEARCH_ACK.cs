using System;

namespace PROTOCOL.GAME
{
	public class GS_MINE_SEARCH_ACK
	{
		public int i32Result;

		public long i64TotalMoney;

		public long i64GuildID;

		public char[] szGuildName = new char[11];

		public int i32MineInfoCount;

		public int i32SolListCount;

		public byte m_nMode;
	}
}
