using System;

namespace PROTOCOL.GAME
{
	public class GS_FRIEND_APPLY_REQ
	{
		public byte ui8ApplyType;

		public short i32WorldID;

		public char[] name = new char[21];

		public char[] FaceBookID = new char[65];

		public char[] PlatformMyName = new char[21];

		public char[] PlatformFriendName = new char[21];
	}
}
