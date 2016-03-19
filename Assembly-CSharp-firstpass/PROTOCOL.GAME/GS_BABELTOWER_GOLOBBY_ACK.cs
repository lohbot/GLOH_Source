using System;

namespace PROTOCOL.GAME
{
	public class GS_BABELTOWER_GOLOBBY_ACK
	{
		public int result;

		public short babel_floor;

		public short babel_subfloor;

		public int nBabelRoomIndex;

		public long nLeaderPersonID;

		public long nEnterPersonID;

		public short nMinLevel;

		public short nMaxLevel;

		public BABELTOWER_PERSON_PACKET[] stBabelPersonInfo = new BABELTOWER_PERSON_PACKET[4];

		public short i16BountyHuntUnique;

		public short i16babel_floortype;

		public GS_BABELTOWER_GOLOBBY_ACK()
		{
			for (int i = 0; i < 4; i++)
			{
				this.stBabelPersonInfo[i] = new BABELTOWER_PERSON_PACKET();
			}
		}
	}
}
