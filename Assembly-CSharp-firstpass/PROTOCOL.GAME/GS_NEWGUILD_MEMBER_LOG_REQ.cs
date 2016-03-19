using System;

namespace PROTOCOL.GAME
{
	public class GS_NEWGUILD_MEMBER_LOG_REQ
	{
		public short i16ChannelID;

		public int i32MapUnique;

		public char[] strMapName = new char[128];
	}
}
