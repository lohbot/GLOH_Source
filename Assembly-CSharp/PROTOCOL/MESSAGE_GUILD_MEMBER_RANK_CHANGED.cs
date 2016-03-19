using System;

namespace PROTOCOL
{
	public class MESSAGE_GUILD_MEMBER_RANK_CHANGED
	{
		public char[] szCharName = new char[21];

		public byte byRank;
	}
}
