using System;

namespace PROTOCOL
{
	public class MINE_GUILD_CURRENTSTATUS_INFO
	{
		public long i64MineID;

		public short i16MineDataID;

		public byte byMilitaryState;

		public long i64AttackGuildID;

		public char[] szAttackGuildName = new char[11];

		public byte byAttackMilitaryCount;

		public long i64DefenceGuildID;

		public char[] szDefenceGuildName = new char[11];

		public byte byDefenceMilitaryCount;

		public long i64LeftTime;

		public byte byCurrentStatusType;

		public byte byJoinMilitary;
	}
}
