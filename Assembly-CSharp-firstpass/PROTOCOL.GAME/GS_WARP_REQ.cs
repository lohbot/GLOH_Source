using System;

namespace PROTOCOL.GAME
{
	public class GS_WARP_REQ
	{
		public byte nMode;

		public int nGateIndex;

		public int nWorldMapWarp_MapIDX;

		public int nIndunIndex = -1;

		public GS_WARP_REQ()
		{
			this.nMode = 0;
			this.nGateIndex = 0;
			this.nWorldMapWarp_MapIDX = 0;
			this.nIndunIndex = -1;
		}
	}
}
