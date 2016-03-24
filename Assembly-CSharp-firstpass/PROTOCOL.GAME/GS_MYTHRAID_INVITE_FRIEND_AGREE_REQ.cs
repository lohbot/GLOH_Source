using System;

namespace PROTOCOL.GAME
{
	public class GS_MYTHRAID_INVITE_FRIEND_AGREE_REQ
	{
		public byte nInvite;

		public bool bAccept;

		public bool bMoveWorld;

		public int WorldID;

		public byte ChannelID;

		public long PersonID;
	}
}
