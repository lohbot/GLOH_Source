using System;

namespace IndunTriggerClient
{
	public class IndunArea
	{
		public string m_szName = string.Empty;

		public int m_AreaUnique;

		public float m_StartX;

		public float m_StartY;

		public float m_EndX;

		public float m_EndY;

		public bool m_bDelete;

		public IndunArea()
		{
			this.m_szName = string.Empty;
			this.m_bDelete = false;
		}
	}
}
