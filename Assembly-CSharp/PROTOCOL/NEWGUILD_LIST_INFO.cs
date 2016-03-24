using System;

namespace PROTOCOL
{
	public class NEWGUILD_LIST_INFO
	{
		public long i64GuildID;

		public short i16Rank;

		public char[] strGuildName = new char[11];

		public int i32Point;

		public short i16Level;

		public short i16CurGuildNum;

		public short i16MaxGuildNum;

		public char[] strMasterName = new char[11];

		public short i16AgitLevel;

		public bool bGuildWar;
	}
}
