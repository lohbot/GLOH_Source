using System;

namespace PROTOCOL.GAME
{
	public class GS_WHISPER_INVITE_ACK
	{
		public byte i8BlockUser;

		public int nRoomUnique;

		public long nSendInvitePersonID;

		public char[] sSendInvitePersonName = new char[21];

		public char[] szBlockUserName = new char[21];
	}
}
