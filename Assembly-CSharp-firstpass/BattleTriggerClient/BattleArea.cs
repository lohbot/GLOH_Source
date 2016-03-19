using System;

namespace BattleTriggerClient
{
	public class BattleArea
	{
		public string m_szName = string.Empty;

		public int m_AreaUnique;

		public float m_StartX;

		public float m_StartY;

		public float m_EndX;

		public float m_EndY;

		public bool m_bDelete;

		public BattleArea()
		{
			this.m_szName = string.Empty;
			this.m_bDelete = false;
		}
	}
}
