using System;

namespace PROTOCOL.GAME
{
	public class GS_MYTHRAID_GOLOBBY_REQ
	{
		public enum eMode
		{
			eMode_Create,
			eMode_JoinInvite,
			eMode_Join,
			eMode_QuickJoin,
			eMode_CreateInvite
		}

		public byte mode;

		public byte difficulty;

		public long nPersonID;
	}
}
