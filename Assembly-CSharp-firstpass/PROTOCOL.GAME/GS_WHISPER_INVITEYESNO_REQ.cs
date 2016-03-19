using System;

namespace PROTOCOL.GAME
{
	public class GS_WHISPER_INVITEYESNO_REQ
	{
		public enum eWHISPER_INVITEYESNO
		{
			eYES,
			eNO
		}

		public int nRoomUnique;

		public long nSendInvitePersonID;

		public byte i8YesNo;
	}
}
