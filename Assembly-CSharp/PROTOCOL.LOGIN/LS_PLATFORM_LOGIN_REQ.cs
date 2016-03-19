using System;

namespace PROTOCOL.LOGIN
{
	public class LS_PLATFORM_LOGIN_REQ
	{
		public int nAuthPlatformType;

		public int nPlayerPlatformType;

		public int nStoreType;

		public char[] szEncPlatformID = new char[256];

		public char[] szAccountID = new char[65];

		public char[] szDeviceID = new char[513];
	}
}
