using System;

namespace PROTOCOL.GAME
{
	public class GS_SET_SOLDIER_MILLITARY_ACK
	{
		public int nResult;

		public long nLegionID;

		public long nLegionActionID;

		public byte nMilitaryUnique;

		public byte nState;

		public byte nLeaderMilitary;

		public GS_SOLDIER_CHANGE_POSTYPE_ACK[] MilitaryInfo = new GS_SOLDIER_CHANGE_POSTYPE_ACK[5];

		public long i64TotalCount_MineDayJoin;

		public int i32GuildPushCount;

		public GS_SET_SOLDIER_MILLITARY_ACK()
		{
			for (int i = 0; i < 5; i++)
			{
				this.MilitaryInfo[i] = new GS_SOLDIER_CHANGE_POSTYPE_ACK();
			}
		}
	}
}
