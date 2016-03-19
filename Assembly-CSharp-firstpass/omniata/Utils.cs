using System;
using System.Globalization;

namespace omniata
{
	public class Utils
	{
		private static string API = "api.omniata.com";

		private static string TEST_API = "api.omniata.com";

		private static string GetProtocol(bool useSSL)
		{
			return (!useSSL) ? "http://" : "https://";
		}

		public static string GetEventAPI(bool useSSL, bool debug)
		{
			if (debug)
			{
				return Utils.GetProtocol(useSSL) + Utils.TEST_API + "/event";
			}
			return Utils.GetProtocol(useSSL) + Utils.API + "/event";
		}

		public static string GetChannelAPI(bool useSSL, bool debug)
		{
			if (debug)
			{
				return Utils.GetProtocol(useSSL) + Utils.TEST_API + "/channel";
			}
			return Utils.GetProtocol(useSSL) + Utils.API + "/channel";
		}

		public static string DoubleToIntegerString(double value)
		{
			return value.ToString("0", CultureInfo.InvariantCulture);
		}

		public static string DecimalToString(decimal value)
		{
			return value.ToString("0.###", CultureInfo.InvariantCulture);
		}

		public static double SecondsSinceEpoch()
		{
			return DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
		}
	}
}
