using System;

namespace PROTOCOL.GAME
{
	public class GS_BATTLE_RESULT_PLUNDER_NFY
	{
		public byte i8WinAlly;

		public float fBattleTime;

		public byte NumSoldiers;

		public long nRewardMoney;

		public int nCurrentMatchPoint;

		public int nAddMatchPoint;

		public char[] szAttackerName = new char[21];

		public char[] szDefencerName = new char[21];

		public bool[] bAttackDeadStartPos = new bool[3];
	}
}
