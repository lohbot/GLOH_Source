using System;

namespace PROTOCOL.GAME
{
	public class GS_NEWGUILD_BOSS_STARTBATTLE_REQ
	{
		public short i16Floor;

		public GS_GUILDBOSS_SOL_BATTLEPOS[] clSolBatchPosInfo = new GS_GUILDBOSS_SOL_BATTLEPOS[9];

		public GS_NEWGUILD_BOSS_STARTBATTLE_REQ()
		{
			for (int i = 0; i < 9; i++)
			{
				this.clSolBatchPosInfo[i] = new GS_GUILDBOSS_SOL_BATTLEPOS();
			}
		}
	}
}
