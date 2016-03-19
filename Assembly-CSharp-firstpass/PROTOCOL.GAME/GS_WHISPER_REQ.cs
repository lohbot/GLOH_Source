using System;

namespace PROTOCOL.GAME
{
	public class GS_WHISPER_REQ
	{
		public int RoomUnique;

		public char[] Name = new char[21];

		public char[] ChatMsg = new char[100];

		public short Color;
	}
}
