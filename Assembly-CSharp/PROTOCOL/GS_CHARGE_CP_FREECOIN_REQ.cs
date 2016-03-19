using System;

namespace PROTOCOL
{
	public class GS_CHARGE_CP_FREECOIN_REQ
	{
		public char[] szCPID = new char[11];

		public char[] szUserID = new char[256];

		public char[] szParamData = new char[128];
	}
}
