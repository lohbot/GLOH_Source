using System;

namespace PROTOCOL.GAME
{
	public class GS_MYTHRAID_LEAVE_ACK
	{
		public long nLeavePersonID;

		public char[] szCharName = new char[21];

		public byte mode;

		public short nSolCount;
	}
}
