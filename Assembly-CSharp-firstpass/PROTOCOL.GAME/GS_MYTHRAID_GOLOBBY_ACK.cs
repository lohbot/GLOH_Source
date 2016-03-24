using System;

namespace PROTOCOL.GAME
{
	public class GS_MYTHRAID_GOLOBBY_ACK
	{
		public int result;

		public byte enumDifficulty;

		public int nBabelRoomIndex;

		public long nLeaderPersonID;

		public long nEnterPersonID;

		public short nMinLevel;

		public short nMaxLevel;

		public MYTHRAID_PERSON_PACKET[] stMythRaidPersonInfo = new MYTHRAID_PERSON_PACKET[4];

		public byte i8Season;

		public GS_MYTHRAID_GOLOBBY_ACK()
		{
			for (int i = 0; i < 4; i++)
			{
				this.stMythRaidPersonInfo[i] = new MYTHRAID_PERSON_PACKET();
			}
		}
	}
}
