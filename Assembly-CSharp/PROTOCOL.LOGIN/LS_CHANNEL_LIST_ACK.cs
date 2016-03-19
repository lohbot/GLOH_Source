using System;

namespace PROTOCOL.LOGIN
{
	public class LS_CHANNEL_LIST_ACK
	{
		public class CHANNEL
		{
			public byte CHID;

			public char[] Name = new char[51];

			public byte State;
		}

		public byte NumChannels;
	}
}
