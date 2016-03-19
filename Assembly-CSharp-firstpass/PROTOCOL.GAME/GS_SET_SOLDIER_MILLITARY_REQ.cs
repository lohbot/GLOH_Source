using System;

namespace PROTOCOL.GAME
{
	public class GS_SET_SOLDIER_MILLITARY_REQ
	{
		public byte m_nMode;

		public long m_nGuildID;

		public long m_nMineID;

		public byte m_nMineGrade;

		public GS_SOLDIER_CHANGE_POSTYPE_REQ[] MilitaryInfo = new GS_SOLDIER_CHANGE_POSTYPE_REQ[5];

		public GS_SET_SOLDIER_MILLITARY_REQ()
		{
			for (int i = 0; i < 5; i++)
			{
				this.MilitaryInfo[i] = new GS_SOLDIER_CHANGE_POSTYPE_REQ();
			}
		}
	}
}
