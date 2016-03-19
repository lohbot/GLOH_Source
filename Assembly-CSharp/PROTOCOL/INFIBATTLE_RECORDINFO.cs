using System;

namespace PROTOCOL
{
	public class INFIBATTLE_RECORDINFO
	{
		public long i64InfinityBattleRecordID;

		public long i64AttackPersonID;

		public char[] szAttackCharName = new char[21];

		public short i16AttackCharLevel;

		public long i64DefencePersonID;

		public char[] szDefenceCharName = new char[21];

		public short i16DefenceCharLevel;

		public long i64BattleTime;

		public byte ui8WinType;

		public int i32NewRank;

		public int i32OldRank;
	}
}
