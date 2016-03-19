using System;

namespace TsPatch
{
	public static class Platform
	{
		public enum Type
		{
			UNKOWN,
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
	}
}
