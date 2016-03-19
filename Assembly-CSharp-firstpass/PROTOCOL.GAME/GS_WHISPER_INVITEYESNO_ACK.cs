using System;

namespace PROTOCOL.GAME
{
	public class GS_WHISPER_INVITEYESNO_ACK
	{
		public enum eWHISPER_INVITEYESNO
		{
			eYES,
			eNO
		}

		public int i64RoomUnique;

		public long nSendInvitePersonID;

		public long nReceiveInvitePersonID;

		public byte i8YesNo;

		public char[] szReceiveInvitePersonName = new char[21];
	}
}
