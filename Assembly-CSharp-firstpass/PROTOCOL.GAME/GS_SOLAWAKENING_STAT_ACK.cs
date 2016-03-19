using System;

namespace PROTOCOL.GAME
{
	public class GS_SOLAWAKENING_STAT_ACK
	{
		public long i64SolID;

		public int[] i32SolSubDataType = new int[3];

		public long[] i64SolSubDataValue = new long[3];

		public int[] i32AwakeningStat = new int[4];

		public bool bRingSlotOpen;

		public int i32Result = -1;
	}
}
