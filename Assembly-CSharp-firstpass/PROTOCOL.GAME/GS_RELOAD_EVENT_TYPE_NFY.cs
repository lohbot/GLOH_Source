using System;

namespace PROTOCOL.GAME
{
	public class GS_RELOAD_EVENT_TYPE_NFY
	{
		public int[] m_nEventType = new int[6];

		public int[] m_nStartTime = new int[6];

		public int[] m_nEndDurationTime = new int[6];

		public int[] m_nMaxLimitCount = new int[6];

		public long[] m_nLeftEventTime = new long[6];

		public byte m_nPushNotice;

		public int m_nDay;

		public int m_nWeek;
	}
}
