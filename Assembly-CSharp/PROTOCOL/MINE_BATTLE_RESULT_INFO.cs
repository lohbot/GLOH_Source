using System;

namespace PROTOCOL
{
	public class MINE_BATTLE_RESULT_INFO
	{
		public long i64LegionActionID;

		public int i32ItemUnique;

		public int i32ItemNum;

		public short i16MineDataID;

		public long i64BattleTime;

		public char[] szEnemyGuildName = new char[11];

		public bool bWin;

		public bool bAttack;

		public bool bMeveBack;

		public bool bIsHiddenName;
	}
}
