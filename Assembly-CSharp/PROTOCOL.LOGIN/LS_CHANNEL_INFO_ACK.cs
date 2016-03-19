using System;

namespace PROTOCOL.LOGIN
{
	public class LS_CHANNEL_INFO_ACK
	{
		public enum eRESULT
		{
			R_OK,
			NOT_FOUND
		}

		public LS_CHANNEL_INFO_ACK.eRESULT Result;

		public char[] IP = new char[16];

		public ushort Port;
	}
}
