using System;

namespace PROTOCOL.GAME
{
	public class GS_BABELTOWER_INVITE_FRIEND_ACK
	{
		public int Result;

		public short floor;

		public short sub_floor;

		public long ReqPersonID;

		public int nReqPersonWorldID;

		public byte ReqPersonCHID;

		public char[] ReqPersonName = new char[21];

		public long InvitePersonID;

		public char[] InvitePersonName = new char[21];

		public short i16BountyHuntUnique;

		public short floortype;
	}
}
