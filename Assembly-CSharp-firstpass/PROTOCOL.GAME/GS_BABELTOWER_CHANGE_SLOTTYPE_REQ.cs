using System;

namespace PROTOCOL.GAME
{
	public class GS_BABELTOWER_CHANGE_SLOTTYPE_REQ
	{
		public enum eCHANGE_TYPE
		{
			eCHANGE_TYPE_CLOSE,
			eCHANGE_TYPE_FRIEND,
			eCHANGE_TYPE_ALLUSER,
			eCHANGE_TYPE_MAX
		}

		public byte change_type;

		public int nBabelRoomIndex;

		public int pos;
	}
}
