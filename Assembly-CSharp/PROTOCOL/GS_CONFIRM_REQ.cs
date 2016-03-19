using System;

namespace PROTOCOL
{
	public class GS_CONFIRM_REQ
	{
		public char[] szUserID = new char[256];

		public char[] szParamData = new char[1024];
	}
}
