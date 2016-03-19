using System;

namespace PROTOCOL.LOGIN
{
	public class LS_CHINA_LOGIN_REQ
	{
		public int nAuthPlatformType;

		public int nPlayerPlatformType;

		public int nStoreType;

		public char[] szDeviceID = new char[513];

		public char[] szParamData = new char[2048];
	}
}
