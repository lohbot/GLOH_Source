using GAME;
using System;

namespace PROTOCOL.GAME
{
	public class GS_PLATFORM_FRIENDINFO_NAME_UPDATE_REQ
	{
		public long nMyPersonID;

		public char[] MyCharName = new char[21];

		public PLATFORM_FRIEND_NAME[] FriendNames = new PLATFORM_FRIEND_NAME[30];

		public byte byCount;
	}
}
