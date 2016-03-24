using System;

namespace PROTOCOL.GAME
{
	public class GS_INFIBATTLE_REWARDINFO_ACK
	{
		public INFIBATTLE_TOPRANK MyRankData = new INFIBATTLE_TOPRANK();

		public byte i8SubDataCount;

		public int i32Result;
	}
}
