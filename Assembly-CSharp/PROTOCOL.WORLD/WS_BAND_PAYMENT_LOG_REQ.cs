using System;

namespace PROTOCOL.WORLD
{
	public class WS_BAND_PAYMENT_LOG_REQ
	{
		public char[] Receipt_id = new char[4000];

		public char[] User_key = new char[65];

		public char[] MarketType = new char[10];

		public char[] Os_Type = new char[10];

		public float Price;
	}
}
