using System;
using UnityEngine;

namespace Chartboost
{
	public class CBBinding
	{
		private static AndroidJavaObject _plugin;

		private static bool initialized;

		static CBBinding()
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return;
			}
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.chartboost.sdk.unity.CBPlugin"))
			{
				CBBinding._plugin = androidJavaClass.CallStatic<AndroidJavaObject>("instance", new object[0]);
			}
		}

		private static bool checkInitialized()
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return false;
			}
			if (CBBinding.initialized)
			{
				return true;
			}
			Debug.Log("Please call CBBinding.init() before using any other features of this library.");
			return false;
		}

		public static void init(string appId, string appSignature)
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				CBBinding._plugin.Call("init", new object[]
				{
					appId,
					appSignature
				});
			}
			CBBinding.initialized = true;
		}

		public static void pause(bool paused)
		{
			if (!CBBinding.checkInitialized())
			{
				return;
			}
			CBBinding._plugin.Call("pause", new object[]
			{
				paused
			});
		}

		public static void destroy()
		{
			if (!CBBinding.checkInitialized())
			{
				return;
			}
			CBBinding._plugin.Call("destroy", new object[0]);
			CBBinding.initialized = false;
		}

		public static bool onBackPressed()
		{
			return CBBinding.checkInitialized() && CBBinding._plugin.Call<bool>("onBackPressed", new object[0]);
		}

		public static bool isImpressionVisible()
		{
			return CBBinding.checkInitialized() && CBManager.isImpressionVisible();
		}

		public static void cacheInterstitial(string location)
		{
			if (!CBBinding.checkInitialized())
			{
				return;
			}
			if (location == null)
			{
				location = string.Empty;
			}
			CBBinding._plugin.Call("cacheInterstitial", new object[]
			{
				location
			});
		}

		public static bool hasCachedInterstitial(string location)
		{
			if (!CBBinding.checkInitialized())
			{
				return false;
			}
			if (location == null)
			{
				location = string.Empty;
			}
			if (location == null)
			{
				location = string.Empty;
			}
			return CBBinding._plugin.Call<bool>("hasCachedInterstitial", new object[]
			{
				location
			});
		}

		public static void showInterstitial(string location)
		{
			if (!CBBinding.checkInitialized())
			{
				return;
			}
			if (location == null)
			{
				location = string.Empty;
			}
			CBBinding._plugin.Call("showInterstitial", new object[]
			{
				location
			});
		}

		public static void cacheMoreApps()
		{
			if (!CBBinding.checkInitialized())
			{
				return;
			}
			CBBinding._plugin.Call("cacheMoreApps", new object[0]);
		}

		public static bool hasCachedMoreApps()
		{
			return CBBinding.checkInitialized() && CBBinding._plugin.Call<bool>("hasCachedMoreApps", new object[0]);
		}

		public static void showMoreApps()
		{
			if (!CBBinding.checkInitialized())
			{
				return;
			}
			CBBinding._plugin.Call("showMoreApps", new object[0]);
		}

		public static void forceOrientation(ScreenOrientation orient)
		{
			if (!CBBinding.checkInitialized())
			{
				return;
			}
			CBBinding._plugin.Call("forceOrientation", new object[]
			{
				orient.ToString()
			});
		}
	}
}
