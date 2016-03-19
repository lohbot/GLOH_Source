using System;

namespace PROTOCOL.GAME
{
	public class GS_PLUNDER_BATTLE_START_REQ
	{
		public long[] m_nSolID = new long[15];

		public long[] m_nSolSubData = new long[15];

		public GS_PLUNDER_BATTLE_START_REQ()
		{
			for (int i = 0; i < 15; i++)
			{
				this.m_nSolID[i] = 0L;
				this.m_nSolSubData[i] = 0L;
			}
		}
	}
}
