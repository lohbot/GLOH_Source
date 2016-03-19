using System;

namespace PROTOCOL
{
	public class GS_USE_COIN_REQ
	{
		public char[] szCPID = new char[11];

		public char[] szUserID = new char[256];

		public char[] szParamData = new char[128];
	}
}
