using System;

namespace BattleTriggerClient
{
	public class BattleTriggerEventBase
	{
		public string m_szName = string.Empty;

		public int m_EventUnique;

		public int m_EventTitleTextNum;

		public int m_iContentTextNum;

		public int m_iTriggerNum;

		public int m_iActionNum;

		public bool m_bAction;

		public bool m_bShow;

		public void Init()
		{
			this.m_EventUnique = 0;
			this.m_EventTitleTextNum = 0;
			this.m_iContentTextNum = 0;
			this.m_iTriggerNum = 0;
			this.m_iActionNum = 0;
			this.m_bAction = false;
			this.m_bShow = false;
			this.m_szName = string.Empty;
		}
	}
}
