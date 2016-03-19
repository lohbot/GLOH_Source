using System;

namespace PROTOCOL.GAME
{
	public class GS_MINE_MILITRAY_CHANGE_POS_REQ
	{
		public long i64MineID;

		public long nGuildID;

		public MILITRAY_CHANGE_POS_INFO[] MilitaryBatchInfo = new MILITRAY_CHANGE_POS_INFO[9];

		public GS_MINE_MILITRAY_CHANGE_POS_REQ()
		{
			for (int i = 0; i < 9; i++)
			{
				this.MilitaryBatchInfo[i] = new MILITRAY_CHANGE_POS_INFO();
			}
		}
	}
}
