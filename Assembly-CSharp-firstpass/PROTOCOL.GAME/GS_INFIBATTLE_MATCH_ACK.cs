using System;

namespace PROTOCOL.GAME
{
	public class GS_INFIBATTLE_MATCH_ACK
	{
		public char[] strDefenseName = new char[21];

		public short i16CharLevel;

		public PLUNDER_TARGET_INFO[] DefenseSolInfo = new PLUNDER_TARGET_INFO[15];

		public int i32Rank;

		public int i32StraightWin;

		public int i32Result;

		public GS_INFIBATTLE_MATCH_ACK()
		{
			for (int i = 0; i < 15; i++)
			{
				this.DefenseSolInfo[i] = new PLUNDER_TARGET_INFO();
			}
		}
	}
}
