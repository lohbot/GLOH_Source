using System;

namespace PROTOCOL.GAME
{
	public class GS_BILLINGCHECK_REQ
	{
		public long SN;

		public long i64ItemMallIndex;

		public char[] UniqueCode = new char[128];

		public char[] Receipt = new char[4000];
	}
}
