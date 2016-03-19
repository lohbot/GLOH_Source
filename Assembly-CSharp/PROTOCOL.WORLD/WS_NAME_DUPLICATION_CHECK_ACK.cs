using System;

namespace PROTOCOL.WORLD
{
	public class WS_NAME_DUPLICATION_CHECK_ACK
	{
		public enum eRESULT
		{
			R_OK,
			R_FAIL
		}

		public byte Result;

		public char[] szCharName = new char[21];

		public WS_NAME_DUPLICATION_CHECK_ACK()
		{
			this.Result = 1;
		}
	}
}
