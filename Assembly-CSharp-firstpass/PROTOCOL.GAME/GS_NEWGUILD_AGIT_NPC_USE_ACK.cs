using System;

namespace PROTOCOL.GAME
{
	public class GS_NEWGUILD_AGIT_NPC_USE_ACK
	{
		public byte ui8NPCType;

		public short i16SellType;

		public int i32SubdataType;

		public long i64SubdataValue;

		public int i32Result = -1;
	}
}
