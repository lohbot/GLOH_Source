using System;

namespace PROTOCOL.WORLD
{
	public class WS_PLATFORM_INVITE_FRIENDS_REQ
	{
		public byte PlatformType;

		public char[] Access_Token = new char[101];

		public char[] Friend_Key = new char[65];
	}
}
