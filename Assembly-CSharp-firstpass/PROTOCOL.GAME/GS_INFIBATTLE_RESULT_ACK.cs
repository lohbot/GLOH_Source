using System;

namespace PROTOCOL.GAME
{
	public class GS_INFIBATTLE_RESULT_ACK
	{
		public byte i8WinAlly;

		public float fBattleTime;

		public byte i8NumSoldiers;

		public int i32RewardItemUnique;

		public int i32RewardItemNum;

		public int i32StraightWinItemUnique;

		public int i32StraightWinItemNum;

		public int i32StraightLoseItemUnique;

		public int i32StraightLoseItemNum;

		public int i32AttackRank;

		public int i32DefenseRank;

		public int i32AttackWinCount;

		public int i32AttackLoseCount;

		public char[] strAttackName = new char[21];

		public char[] strDefencerName = new char[21];

		public bool[] bAttackDeadStartPos = new bool[3];
	}
}
