using PROTOCOL.BASE;
using System;

namespace PROTOCOL.GAME
{
	public class CLOSE_SESSION_REQ
	{
		public enum eCLOSE_MODE : byte
		{
			CLOSE_SESSION,
			CHANGE_CHANNEL
		}

		public PacketHeader header;

		public byte Mode;

		public CLOSE_SESSION_REQ()
		{
			this.header = new PacketHeader(base.GetType(), 3);
		}
	}
}
