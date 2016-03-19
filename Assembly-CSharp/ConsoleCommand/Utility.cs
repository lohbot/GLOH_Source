using System;

namespace ConsoleCommand
{
	public class Utility
	{
		public static bool TryParseBoolean(string str, out bool active)
		{
			if (string.Compare(str, "on", true) == 0)
			{
				active = true;
				return true;
			}
			if (string.Compare(str, "off", true) == 0)
			{
				active = false;
				return true;
			}
			active = false;
			return false;
		}

		public static bool ParseBoolean(string str, bool defaultValue = true)
		{
			return string.Compare(str, "on", true) == 0 || (string.Compare(str, "off", true) != 0 && defaultValue);
		}
	}
}
