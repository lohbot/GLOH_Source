using System;

namespace PROTOCOL.LOGIN
{
	public class LS_LOGIN_USER_REQ
	{
		public int nAuthPlatformType;

		public int nPlayerPlatformType;

		public int nStoreType;

		public char[] szEncAccountID = new char[256];

		public char[] szPassword = new char[41];

		public char[] szDeviceID = new char[513];
	}
}
