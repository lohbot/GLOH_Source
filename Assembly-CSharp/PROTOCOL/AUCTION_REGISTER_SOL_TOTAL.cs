using GAME;
using PROTOCOL.GAME;
using System;

namespace PROTOCOL
{
	public class AUCTION_REGISTER_SOL_TOTAL
	{
		public AUCTION_REGISTER_INFO RegisterInfo = new AUCTION_REGISTER_INFO();

		public SOLDIER_INFO SoldierInfo = new SOLDIER_INFO();

		public BATTLESKILL_DATA[] BattleSkillData = new BATTLESKILL_DATA[6];

		public long[] SolSubData = new long[16];

		public AUCTION_REGISTER_SOL_TOTAL()
		{
			for (int i = 0; i < 6; i++)
			{
				this.BattleSkillData[i] = new BATTLESKILL_DATA();
			}
			for (int i = 0; i < 16; i++)
			{
				this.SolSubData[i] = 0L;
			}
		}
	}
}
