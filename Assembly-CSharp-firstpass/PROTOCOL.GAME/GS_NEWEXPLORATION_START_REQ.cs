using System;

namespace PROTOCOL.GAME
{
	public class GS_NEWEXPLORATION_START_REQ
	{
		public sbyte i8Floor;

		public sbyte i8SubFloor;

		public int i32CombinationUnique;

		public bool bOnBattleSpeed;

		public long[] i64SolID = new long[5];

		public byte[] i8SolPosition = new byte[5];

		public GS_NEWEXPLORATION_START_REQ()
		{
			for (int i = 0; i < 5; i++)
			{
				this.i64SolID[i] = 0L;
				this.i8SolPosition[i] = 0;
			}
		}
	}
}
