using System;

namespace PROTOCOL.GAME
{
	public class GS_COSTUME_BUY_ACK
	{
		public int Result;

		public int i32CostumeUnique;

		public char[] strGiftUserName = new char[21];

		public bool bIsGift;
	}
}
