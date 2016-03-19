using System;

namespace PROTOCOL.GAME
{
	public class GS_BILLINGCHECK_REQ
	{
		public long SN;

		public long i64ItemMallIndex;

		public long UniqueCode;

		public char[] Receipt = new char[4000];
	}
}
