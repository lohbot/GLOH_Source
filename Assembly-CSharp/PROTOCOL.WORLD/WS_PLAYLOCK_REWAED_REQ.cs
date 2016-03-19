using System;

namespace PROTOCOL.WORLD
{
	public class WS_PLAYLOCK_REWAED_REQ
	{
		public char[] RequestKey = new char[40];

		public long DeviceID;

		public byte QuestKey;
	}
}
