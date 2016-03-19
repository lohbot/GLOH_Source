using System;

namespace PROTOCOL.GAME
{
	public class GS_GET_CURRENT_EVENT_LIST_ACK
	{
		public sbyte m_nEventInfoWeek;

		public int m_nEventType;

		public int m_nStartTime;

		public int m_nEndEventTime;

		public int m_nMaxLimitCount;

		public long m_nLeftEventTime;

		public int m_nTitleText;

		public int m_nExplain;
	}
}
