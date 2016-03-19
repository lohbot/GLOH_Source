using System;

namespace PROTOCOL.GAME
{
	public class GS_BATTLE_BABELTOWER_CHARINFO_ACK
	{
		public BATTLE_BABELTOWER_CHARINFO[] stBabelCharInfo = new BATTLE_BABELTOWER_CHARINFO[4];

		public GS_BATTLE_BABELTOWER_CHARINFO_ACK()
		{
			for (int i = 0; i < 4; i++)
			{
				this.stBabelCharInfo[i] = new BATTLE_BABELTOWER_CHARINFO();
			}
		}
	}
}
