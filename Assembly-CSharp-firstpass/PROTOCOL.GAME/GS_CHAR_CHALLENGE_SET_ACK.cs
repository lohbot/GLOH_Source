using System;

namespace PROTOCOL.GAME
{
	public class GS_CHAR_CHALLENGE_SET_ACK
	{
		public int m_nResult;

		public Challenge_Info m_kChallengeInfo = new Challenge_Info();

		public int m_nReward;

		public int m_nIndex;

		public int m_nDetailIndex;

		public long m_nDetailValue;
	}
}
