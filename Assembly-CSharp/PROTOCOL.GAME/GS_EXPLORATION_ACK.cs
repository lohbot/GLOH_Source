using GAME;
using System;

namespace PROTOCOL.GAME
{
	public class GS_EXPLORATION_ACK
	{
		public int m_nResult;

		public long m_nAddMoney;

		public ITEM m_kRewardItem = new ITEM();

		public int m_nTableIndex;

		public int m_nIndex;
	}
}
