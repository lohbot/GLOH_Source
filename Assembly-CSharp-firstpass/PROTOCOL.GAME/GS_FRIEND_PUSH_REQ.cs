using System;

namespace PROTOCOL.GAME
{
	public class GS_FRIEND_PUSH_REQ
	{
		public long i64PersonID;

		public long i64FriendPersonID;

		public char[] szChatStr = new char[256];

		public byte i8RecvType;
	}
}
