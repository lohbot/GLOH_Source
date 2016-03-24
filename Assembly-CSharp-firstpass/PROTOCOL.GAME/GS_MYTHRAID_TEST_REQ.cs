using System;

namespace PROTOCOL.GAME
{
	public class GS_MYTHRAID_TEST_REQ
	{
		public enum eMode
		{
			eMode_Easy,
			eMode_Normal,
			eMode_Hard
		}

		public byte mode;
	}
}
