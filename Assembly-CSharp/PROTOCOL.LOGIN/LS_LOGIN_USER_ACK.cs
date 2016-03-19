using System;

namespace PROTOCOL.LOGIN
{
	public class LS_LOGIN_USER_ACK
	{
		public int Result;

		public char[] szAuthKey = new char[37];

		public long nSerialNumber;
	}
}
