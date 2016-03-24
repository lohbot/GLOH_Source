using System;

namespace PROTOCOL.GAME
{
	public class GS_MINE_BATTLE_RESULT_REWARD_GET_ALL_REQ
	{
		public long[] i64LegionActionIDs = new long[5];

		public GS_MINE_BATTLE_RESULT_REWARD_GET_ALL_REQ()
		{
			for (int i = 0; i < this.i64LegionActionIDs.Length; i++)
			{
				this.i64LegionActionIDs[i] = 0L;
			}
		}
	}
}
