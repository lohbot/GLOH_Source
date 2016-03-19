using System;

namespace NPatch
{
	public static class HWChecker
	{
		private static IHWChecker _instance;

		public static IHWChecker Instance
		{
			get
			{
				if (HWChecker._instance == null)
				{
					HWChecker._instance = new UnityAndroidChecker();
				}
				return HWChecker._instance;
			}
		}

		public static string IdentifierName
		{
			get;
			set;
		}
	}
}
