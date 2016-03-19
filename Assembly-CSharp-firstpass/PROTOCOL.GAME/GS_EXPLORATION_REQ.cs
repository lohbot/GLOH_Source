using System;

namespace PROTOCOL.GAME
{
	public class GS_EXPLORATION_REQ
	{
		public long[] m_nSolID = new long[6];

		public GS_EXPLORATION_REQ()
		{
			for (int i = 0; i < 6; i++)
			{
				this.m_nSolID[i] = 0L;
			}
		}
	}
}
