using System;

namespace PROTOCOL.WORLD
{
	public class WS_USER_LOGIN_ACK
	{
		public int Result;

		public int SessionKey;

		public int nAuthPlatformType;

		public long UID;

		public short i16WorldID_LoggedIn;

		public char[] szWorldName = new char[32];

		public short i16ChannelID_LoggedIn;

		public long i64AccountWorldInfoKey;

		public long i64PersonID_LoggedIn;

		public char[] szNewAuthKey = new char[37];

		public byte ui8BlockMode;

		public long tUnblockDate;

		public int nMasterLevel;

		public int nConfirmCheck;

		public int nGetConfirmItem;
	}
}
