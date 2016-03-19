using System;

namespace PROTOCOL.GAME
{
	public class GS_ITEM_LOCK_MULTI_ACK
	{
		public int i32Result;

		public long[] i64ItemID = new long[30];

		public bool[] bLocked = new bool[30];

		public GS_ITEM_LOCK_MULTI_ACK()
		{
			for (int i = 0; i < 30; i++)
			{
				this.i64ItemID[i] = 0L;
				this.bLocked[i] = false;
			}
		}
	}
}
