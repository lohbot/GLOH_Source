using System;

namespace PROTOCOL.GAME
{
	public class GS_SET_SOLDIER_BATTLESKILL_ACK
	{
		public int nResult;

		public long nSolID;

		public int nBattleSkillIndex;

		public int nBattleSkillUnique;

		public int nBattleSkillLevel;

		public long lLeftMoney;

		public bool bBattleSkillMessageShow;
	}
}
