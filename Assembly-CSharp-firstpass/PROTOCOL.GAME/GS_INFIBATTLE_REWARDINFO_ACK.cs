using System;

namespace PROTOCOL.GAME
{
	public class GS_INFIBATTLE_REWARDINFO_ACK
	{
		public int i32OldRank;

		public int[] i32Rank = new int[9];

		public int[] i32RewardUnique = new int[9];

		public short[] i16RewardNum = new short[9];

		public int i32Result;
	}
}
