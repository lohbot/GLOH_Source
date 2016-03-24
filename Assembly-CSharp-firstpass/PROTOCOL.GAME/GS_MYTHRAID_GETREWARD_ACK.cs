using System;

namespace PROTOCOL.GAME
{
	public class GS_MYTHRAID_GETREWARD_ACK
	{
		public int[] i32itempos = new int[7];

		public int[] i32RewardUnique = new int[7];

		public int[] i32RewardNum = new int[7];

		public byte i8AskType;

		public int i32Result;

		public int i32RewardInfo;

		public byte i8RaidType;
	}
}
