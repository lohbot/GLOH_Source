using System;

namespace PROTOCOL.WORLD
{
	public class WS_CHARACTER_LOGIN_BYPASSINFO_SET_NFY
	{
		public short SSNID_USER;

		public long PersonID;

		public char[] CharName = new char[21];

		public short CharLevel;
	}
}
