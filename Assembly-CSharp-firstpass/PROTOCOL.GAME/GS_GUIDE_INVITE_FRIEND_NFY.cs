using System;

namespace PROTOCOL.GAME
{
	public class GS_GUIDE_INVITE_FRIEND_NFY
	{
		public int nResult;

		public byte ui8ReqType;

		public long nPersonID;

		public int nLevel;

		public int nFaceCharKind;

		public char[] InviteName = new char[21];

		public char[] InviteIntroMsg = new char[41];

		public byte ui8Invite_type;

		public int nInvitePerson_FriendCount;
	}
}
