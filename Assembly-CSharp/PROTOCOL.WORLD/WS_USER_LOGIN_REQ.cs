using System;

namespace PROTOCOL.WORLD
{
	public class WS_USER_LOGIN_REQ
	{
		public int nAuthPlatformType;

		public int nPlayerPlatformType;

		public int nStoreType;

		public char[] szAuthKey = new char[37];

		public long nSerialNumber;

		public char[] szAccountID = new char[65];

		public char[] szDeviceToken = new char[513];

		public byte nClientPatchLevel;
	}
}
