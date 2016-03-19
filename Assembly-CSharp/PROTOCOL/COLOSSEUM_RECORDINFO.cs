using System;

namespace PROTOCOL
{
	public class COLOSSEUM_RECORDINFO
	{
		public long i64Colosseum_battleunique;

		public long i64BattleTime;

		public long i64TargetPersonID;

		public short iTargetLevel;

		public char[] szTargetName = new char[21];

		public long i64WinPersonID;
	}
}
