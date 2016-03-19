using System;

namespace PROTOCOL.GAME
{
	public class GS_PLUNDER_MATCH_PLAYER_INFO_ACK
	{
		public char[] m_szTargetLeaderName = new char[21];

		public int m_nTargetLeaderLevel;

		public long m_nMoney;

		public long m_nSellMoney;

		public PLUNDER_TARGET_INFO[] stTagetSolInfo = new PLUNDER_TARGET_INFO[15];

		public long m_nPlunderObjectPos;

		public GS_PLUNDER_MATCH_PLAYER_INFO_ACK()
		{
			for (int i = 0; i < 15; i++)
			{
				this.stTagetSolInfo[i] = new PLUNDER_TARGET_INFO();
			}
		}
	}
}
