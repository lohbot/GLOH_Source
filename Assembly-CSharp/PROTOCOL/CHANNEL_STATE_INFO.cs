using System;

namespace PROTOCOL
{
	public class CHANNEL_STATE_INFO
	{
		public short ChannelID;

		public char[] ChannelName = new char[51];

		public byte State;

		public int UserCount;
	}
}
