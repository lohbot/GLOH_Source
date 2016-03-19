using System;

namespace PROTOCOL.GAME
{
	public class GS_BABELTOWER_GOLOBBY_REQ
	{
		public enum eMode
		{
			eMode_Create,
			eMode_JoinInvite,
			eMode_Join,
			eMode_QuickJoin
		}

		public byte mode;

		public short babel_floor;

		public short babel_subfloor;

		public long nPersonID;

		public short i16BountyHuntUnique;

		public short Babel_FloorType;
	}
}
