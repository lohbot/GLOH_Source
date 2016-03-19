using System;

namespace PROTOCOL
{
	public class GUILDWAR_APPLY_MILITARY_USER_INFO
	{
		public byte ui8RaidBattlePos;

		public long i64MilitaryID;

		public long i64PersonID;

		public byte ui8MilitaryUnique;

		public char[] szCharName = new char[21];

		public short i16CharLevel;

		public byte ui8Leader;
	}
}
