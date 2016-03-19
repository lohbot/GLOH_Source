using System;

namespace PROTOCOL.GAME
{
	public class CLOSE_SESSION_ACK
	{
		public enum eCLOSE_MODE : byte
		{
			CLOSE_SESSION,
			CHANGE_CHANNEL
		}

		public byte Mode;

		public int SessionKey;
	}
}
