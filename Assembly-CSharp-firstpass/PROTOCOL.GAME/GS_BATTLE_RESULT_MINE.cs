using System;

namespace PROTOCOL.GAME
{
	public class GS_BATTLE_RESULT_MINE
	{
		public long i64LegionActionID;

		public long i64MineID;

		public short i16MineDataID;

		public byte ui8MineGrade;

		public long i64WinGuildID;

		public long i64AttackGuildID;

		public char[] szAttackGuildName = new char[11];

		public long i64DefenceGuildID;

		public char[] szDefenceGuildName = new char[11];

		public long i64BattleTime;

		public bool bGiveComplete;

		public bool bIsWin;
	}
}
