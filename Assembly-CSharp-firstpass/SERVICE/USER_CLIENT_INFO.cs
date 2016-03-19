using System;

namespace SERVICE
{
	public class USER_CLIENT_INFO
	{
		public char[] szClientIP = new char[21];

		public char[] szChannelID = new char[10];

		public char[] szClientVersion = new char[16];

		public char[] szSystemVersion = new char[16];

		public char[] szCountryCode = new char[10];

		public char[] szMapAddress = new char[21];

		public char[] szIMEI = new char[32];

		public char[] szIMSI = new char[32];

		public char[] szDeviceModel = new char[32];

		public char[] szDeviceBrand = new char[32];

		public char[] szDeviceID = new char[32];

		public byte nPlayerPlatformType;

		public byte nAgencyType;

		public byte nIOSCheck;

		public byte nRootCheck;
	}
}
