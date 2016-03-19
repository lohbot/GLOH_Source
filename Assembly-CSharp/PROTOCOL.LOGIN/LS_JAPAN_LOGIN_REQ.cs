using System;

namespace PROTOCOL.LOGIN
{
	public class LS_JAPAN_LOGIN_REQ
	{
		public int nAuthPlatformType;

		public int nPlayerPlatformType;

		public int nStoreType;

		public char[] szUserID = new char[256];

		public char[] szDeviceID = new char[513];

		public char[] szParamData = new char[2048];
	}
}
