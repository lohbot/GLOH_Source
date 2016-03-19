using System;

namespace PROTOCOL.GAME
{
	public class GS_BF_HOPE_TO_ENDTURN_REQ
	{
		public short[] iBFCharUnique = new short[6];

		public GS_BF_HOPE_TO_ENDTURN_REQ()
		{
			for (int i = 0; i < 6; i++)
			{
				this.iBFCharUnique[i] = -1;
			}
		}
	}
}
