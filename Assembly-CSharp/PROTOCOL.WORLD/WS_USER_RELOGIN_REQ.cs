using System;

namespace PROTOCOL.WORLD
{
	public class WS_USER_RELOGIN_REQ
	{
		public long UID;

		public long i64AccountWorldInfoKey;

		public char[] szDeviceToken = new char[513];

		public byte i8HP_AuthRequest;
	}
}
