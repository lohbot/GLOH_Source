using System;

namespace PROTOCOL.GAME
{
	public class GS_BILLING_ITEM_RECODE_REQ
	{
		public byte i8Type;

		public byte i8Result;

		public long i64ItemMallIndex;

		public char[] Message1 = new char[51];

		public char[] Message2 = new char[51];

		public char[] Message3 = new char[51];

		public char[] Message4 = new char[51];
	}
}
