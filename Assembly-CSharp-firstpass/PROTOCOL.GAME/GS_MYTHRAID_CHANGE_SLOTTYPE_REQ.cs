using System;

namespace PROTOCOL.GAME
{
	public class GS_MYTHRAID_CHANGE_SLOTTYPE_REQ
	{
		public enum eCHANGE_TYPE
		{
			eCHANGE_TYPE_CLOSE,
			eCHANGE_TYPE_FRIEND,
			eCHANGE_TYPE_ALLUSER
		}

		public byte change_type;

		public int nMythRaidRoomIndex;

		public int pos;
	}
}
