using System;

namespace NPatch
{
	public static class Platform
	{
		public enum Type
		{
			UNKOWN,
			UNKOWN2,
			COMMON,
			WEB,
			AND,
			IOS,
			TYPE_COUNT
		}

		public static Platform.Type RunPlatform
		{
			get;
			set;
		}

		public static char TypeToChar(Platform.Type ePlatformType)
		{
			switch (ePlatformType)
			{
			case Platform.Type.UNKOWN:
				return '?';
			case Platform.Type.COMMON:
				return 'C';
			case Platform.Type.WEB:
				return 'W';
			case Platform.Type.AND:
				return 'A';
			case Platform.Type.IOS:
				return 'I';
			}
			return '?';
		}

		public static Platform.Type CharToType(char cPlatform)
		{
			switch (cPlatform)
			{
			case '?':
				return Platform.Type.UNKOWN;
			case '@':
			case 'B':
				IL_1F:
				if (cPlatform == 'I')
				{
					return Platform.Type.IOS;
				}
				if (cPlatform != 'W')
				{
					return Platform.Type.UNKOWN;
				}
				return Platform.Type.WEB;
			case 'A':
				return Platform.Type.AND;
			case 'C':
				return Platform.Type.COMMON;
			}
			goto IL_1F;
		}
	}
}
