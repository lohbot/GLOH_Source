using System;

namespace PROTOCOL.WORLD
{
	public class WS_DELETE_CHAR_ACK
	{
		public enum eRESULT
		{
			R_OK,
			R_FAIL
		}

		public sbyte Result;

		public long nPersonID;

		public WS_DELETE_CHAR_ACK()
		{
			this.Result = 1;
			this.nPersonID = -1L;
		}
	}
}
