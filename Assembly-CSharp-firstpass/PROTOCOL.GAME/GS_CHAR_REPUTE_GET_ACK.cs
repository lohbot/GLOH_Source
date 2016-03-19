using System;

namespace PROTOCOL.GAME
{
	public class GS_CHAR_REPUTE_GET_ACK
	{
		public int nResult;

		public short[] nUniuqe = new short[20];

		public int[] nValue = new int[20];

		public int[] nRewardValue = new int[20];
	}
}
