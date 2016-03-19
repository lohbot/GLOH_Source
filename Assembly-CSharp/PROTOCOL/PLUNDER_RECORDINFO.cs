using System;

namespace PROTOCOL
{
	public class PLUNDER_RECORDINFO
	{
		public long PlunderUnique;

		public long i64AttackPersonID;

		public char[] szAttackCharName = new char[21];

		public short i16AttackCharLevel;

		public long i64DefencePersonID;

		public char[] szDefenceCharName = new char[21];

		public short i16DefenceCharLevel;

		public long i64BattleTime;

		public long i64PlunderMoney;

		public byte byWinType;
	}
}
