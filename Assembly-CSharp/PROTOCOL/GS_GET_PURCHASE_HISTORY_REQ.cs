using System;

namespace PROTOCOL
{
	public class GS_GET_PURCHASE_HISTORY_REQ
	{
		public char[] szCPID = new char[11];

		public char[] szUserID = new char[256];

		public char[] szParamData = new char[1024];
	}
}
