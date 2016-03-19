using System;

namespace PROTOCOL.LOGIN
{
	public class LS_LOGOUT_USER_ACK
	{
		public enum eRESULT : byte
		{
			R_OK,
			R_FAIL
		}

		public byte Result;
	}
}
