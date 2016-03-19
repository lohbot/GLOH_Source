using System;

namespace PROTOCOL.GAME
{
	public class GS_MINE_WAIT_MILITARY_INFO_ACK
	{
		public int i32Result;

		public WAIT_GUILD_INFO[] clWaitGuildInfo = new WAIT_GUILD_INFO[10];

		public GS_MINE_WAIT_MILITARY_INFO_ACK()
		{
			for (int i = 0; i < 10; i++)
			{
				this.clWaitGuildInfo[i] = new WAIT_GUILD_INFO();
			}
		}
	}
}
