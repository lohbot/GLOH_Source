using GAME;
using System;

namespace PROTOCOL.GAME
{
	public class GS_ADD_FRIEND_ACK
	{
		public int Result;

		public long nPersonID;

		public USER_FRIEND_INFO pFriend = new USER_FRIEND_INFO();
	}
}
