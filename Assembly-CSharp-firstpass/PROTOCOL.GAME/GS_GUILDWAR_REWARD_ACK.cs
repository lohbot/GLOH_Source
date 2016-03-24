using GAME;
using System;

namespace PROTOCOL.GAME
{
	public class GS_GUILDWAR_REWARD_ACK
	{
		public int i32Result;

		public long CanGetRewardTime;

		public int i32RewardGold;

		public ITEM kRewarditem = new ITEM();

		public long i64CurMoney;
	}
}
