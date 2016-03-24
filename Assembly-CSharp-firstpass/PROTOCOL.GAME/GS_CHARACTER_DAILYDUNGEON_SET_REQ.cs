using System;

namespace PROTOCOL.GAME
{
	public class GS_CHARACTER_DAILYDUNGEON_SET_REQ
	{
		public int i32DayOfWeek;

		public sbyte i8Diff;

		public byte i8IsReset;

		public GS_DAILYDUNGEON_SOL_BATTLEPOS[] clSolBatchPosInfo = new GS_DAILYDUNGEON_SOL_BATTLEPOS[6];

		public int nCombinationUnique;

		public GS_CHARACTER_DAILYDUNGEON_SET_REQ()
		{
			for (int i = 0; i < 6; i++)
			{
				this.clSolBatchPosInfo[i] = new GS_DAILYDUNGEON_SOL_BATTLEPOS();
			}
		}
	}
}
