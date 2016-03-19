using System;

namespace PROTOCOL.WORLD
{
	public class WS_OTPAUTH_ACK
	{
		public int Result;

		public byte nOTPRequestType;

		public char[] szOTPAuthKey = new char[37];
	}
}
