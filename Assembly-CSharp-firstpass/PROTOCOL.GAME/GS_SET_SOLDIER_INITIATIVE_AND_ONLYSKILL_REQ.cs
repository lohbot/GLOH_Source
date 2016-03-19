using System;

namespace PROTOCOL.GAME
{
	public class GS_SET_SOLDIER_INITIATIVE_AND_ONLYSKILL_REQ
	{
		public long[] nSolID = new long[16];

		public int[] nInitiativeValue = new int[16];

		public short[] bOnlySkill = new short[16];
	}
}
