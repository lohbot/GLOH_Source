using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Chartboost
{
	public class CBManager : MonoBehaviour
	{
		public enum CBImpressionError
		{
			Internal,
			InternetUnavailable,
			TooManyConnections,
			WrongOrientation,
			NetworkFailure = 5,
			NoAdFound,
			ImpressionAlreadyVisible = 8,
			NoHostActivity
		}

		private static bool isPaused;

		private static float lastTimeScale;

		public static event Action<string, CBManager.CBImpressionError> didFailToLoadInterstitialEvent
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				CBManager.didFailToLoadInterstitialEvent = (Action<string, CBManager.CBImpressionError>)Delegate.Combine(CBManager.didFailToLoadInterstitialEvent, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				CBManager.didFailToLoadInterstitialEvent = (Action<string, CBManager.CBImpressionError>)Delegate.Remove(CBManager.didFailToLoadInterstitialEvent, value);
			}
		}

		public static event Action<string> didDismissInterstitialEvent
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				CBManager.didDismissInterstitialEvent = (Action<string>)Delegate.Combine(CBManager.didDismissInterstitialEvent, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				CBManager.didDismissInterstitialEvent = (Action<string>)Delegate.Remove(CBManager.didDismissInterstitialEvent, value);
			}
		}

		public static event Action<string> didCloseInterstitialEvent
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				CBManager.didCloseInterstitialEvent = (Action<string>)Delegate.Combine(CBManager.didCloseInterstitialEvent, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				CBManager.didCloseInterstitialEvent = (Action<string>)Delegate.Remove(CBManager.didCloseInterstitialEvent, value);
			}
		}

		public static event Action<string> didClickInterstitialEvent
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				CBManager.didClickInterstitialEvent = (Action<string>)Delegate.Combine(CBManager.didClickInterstitialEvent, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				CBManager.didClickInterstitialEvent = (Action<string>)Delegate.Remove(CBManager.didClickInterstitialEvent, value);
			}
		}

		public static event Action<string> didCacheInterstitialEvent
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				CBManager.didCacheInterstitialEvent = (Action<string>)Delegate.Combine(CBManager.didCacheInterstitialEvent, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				CBManager.didCacheInterstitialEvent = (Action<string>)Delegate.Remove(CBManager.didCacheInterstitialEvent, value);
			}
		}

		public static event Action<string> didShowInterstitialEvent
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				CBManager.didShowInterstitialEvent = (Action<string>)Delegate.Combine(CBManager.didShowInterstitialEvent, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				CBManager.didShowInterstitialEvent = (Action<string>)Delegate.Remove(CBManager.didShowInterstitialEvent, value);
			}
		}

		public static event Action<CBManager.CBImpressionError> didFailToLoadMoreAppsEvent
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				CBManager.didFailToLoadMoreAppsEvent = (Action<CBManager.CBImpressionError>)Delegate.Combine(CBManager.didFailToLoadMoreAppsEvent, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				CBManager.didFailToLoadMoreAppsEvent = (Action<CBManager.CBImpressionError>)Delegate.Remove(CBManager.didFailToLoadMoreAppsEvent, value);
			}
		}

		public static event Action didDismissMoreAppsEvent
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				CBManager.didDismissMoreAppsEvent = (Action)Delegate.Combine(CBManager.didDismissMoreAppsEvent, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				CBManager.didDismissMoreAppsEvent = (Action)Delegate.Remove(CBManager.didDismissMoreAppsEvent, value);
			}
		}

		public static event Action didCloseMoreAppsEvent
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				CBManager.didCloseMoreAppsEvent = (Action)Delegate.Combine(CBManager.didCloseMoreAppsEvent, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				CBManager.didCloseMoreAppsEvent = (Action)Delegate.Remove(CBManager.didCloseMoreAppsEvent, value);
			}
		}

		public static event Action didClickMoreAppsEvent
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				CBManager.didClickMoreAppsEvent = (Action)Delegate.Combine(CBManager.didClickMoreAppsEvent, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				CBManager.didClickMoreAppsEvent = (Action)Delegate.Remove(CBManager.didClickMoreAppsEvent, value);
			}
		}

		public static event Action didCacheMoreAppsEvent
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				CBManager.didCacheMoreAppsEvent = (Action)Delegate.Combine(CBManager.didCacheMoreAppsEvent, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				CBManager.didCacheMoreAppsEvent = (Action)Delegate.Remove(CBManager.didCacheMoreAppsEvent, value);
			}
		}

		public static event Action didShowMoreAppsEvent
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				CBManager.didShowMoreAppsEvent = (Action)Delegate.Combine(CBManager.didShowMoreAppsEvent, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				CBManager.didShowMoreAppsEvent = (Action)Delegate.Remove(CBManager.didShowMoreAppsEvent, value);
			}
		}

		private void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}

		public void Start()
		{
			if (!Application.isPlaying)
			{
				return;
			}
		}

		public void OnEnable()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			string appId = string.Empty;
			string appSignature = string.Empty;
			if (Application.platform != RuntimePlatform.Android)
			{
				return;
			}
			appId = "53b3ad95c26ee417a9d1e3fb";
			appSignature = "88a91a14027104f1f32e7c9a0a898d7c9ad281c9";
			CBBinding.init(appId, appSignature);
		}

		public void didFailToLoadInterstitial(string dataString)
		{
			Hashtable hashtable = (Hashtable)CBJSON.Deserialize(dataString);
			CBManager.CBImpressionError arg = CBManager.impressionErrorFromInt(hashtable["errorCode"]);
			if (CBManager.didFailToLoadInterstitialEvent != null)
			{
				CBManager.didFailToLoadInterstitialEvent(hashtable["location"] as string, arg);
			}
		}

		public void didDismissInterstitial(string location)
		{
			CBManager.doUnityPause(false);
			if (CBManager.didDismissInterstitialEvent != null)
			{
				CBManager.didDismissInterstitialEvent(location);
			}
		}

		public void didClickInterstitial(string location)
		{
			if (CBManager.didClickInterstitialEvent != null)
			{
				CBManager.didClickInterstitialEvent(location);
			}
		}

		public void didCloseInterstitial(string location)
		{
			if (CBManager.didCloseInterstitialEvent != null)
			{
				CBManager.didCloseInterstitialEvent(location);
			}
		}

		public void didCacheInterstitial(string location)
		{
			if (CBManager.didCacheInterstitialEvent != null)
			{
				CBManager.didCacheInterstitialEvent(location);
			}
		}

		public void didShowInterstitial(string location)
		{
			CBManager.doUnityPause(true);
			if (CBManager.didShowInterstitialEvent != null)
			{
				CBManager.didShowInterstitialEvent(location);
			}
		}

		public void didFailToLoadMoreApps(string dataString)
		{
			Hashtable hashtable = (Hashtable)CBJSON.Deserialize(dataString);
			CBManager.CBImpressionError obj = CBManager.impressionErrorFromInt(hashtable["errorCode"]);
			if (CBManager.didFailToLoadMoreAppsEvent != null)
			{
				CBManager.didFailToLoadMoreAppsEvent(obj);
			}
		}

		public void didDismissMoreApps(string empty)
		{
			CBManager.doUnityPause(false);
			if (CBManager.didDismissMoreAppsEvent != null)
			{
				CBManager.didDismissMoreAppsEvent();
			}
		}

		public void didClickMoreApps(string empty)
		{
			if (CBManager.didClickMoreAppsEvent != null)
			{
				CBManager.didClickMoreAppsEvent();
			}
		}

		public void didCloseMoreApps(string empty)
		{
			if (CBManager.didCloseMoreAppsEvent != null)
			{
				CBManager.didCloseMoreAppsEvent();
			}
		}

		public void didCacheMoreApps(string empty)
		{
			if (CBManager.didCacheMoreAppsEvent != null)
			{
				CBManager.didCacheMoreAppsEvent();
			}
		}

		public void didShowMoreApps(string empty)
		{
			CBManager.doUnityPause(true);
			if (CBManager.didShowMoreAppsEvent != null)
			{
				CBManager.didShowMoreAppsEvent();
			}
		}

		private static void doUnityPause(bool pause)
		{
			bool flag = true;
			if (pause)
			{
				if (CBManager.isPaused)
				{
					flag = false;
				}
				CBManager.isPaused = true;
				if (flag)
				{
					CBManager.doCustomPause(pause);
				}
			}
			else
			{
				if (!CBManager.isPaused)
				{
					flag = false;
				}
				CBManager.isPaused = false;
				if (flag)
				{
					CBManager.doCustomPause(pause);
				}
			}
		}

		public static bool isImpressionVisible()
		{
			return CBManager.isPaused;
		}

		private static void doCustomPause(bool pause)
		{
			if (pause)
			{
				CBManager.lastTimeScale = Time.timeScale;
				Time.timeScale = 0f;
			}
			else
			{
				Time.timeScale = CBManager.lastTimeScale;
			}
		}

		private static CBManager.CBImpressionError impressionErrorFromInt(object errorObj)
		{
			bool flag = Application.platform == RuntimePlatform.IPhonePlayer;
			int num;
			try
			{
				num = Convert.ToInt32(errorObj);
			}
			catch
			{
				num = -1;
			}
			if (num < 0 || num > 9 || (flag && num > 8) || num == 4 || num == 7 || (flag && num == 8))
			{
				return CBManager.CBImpressionError.Internal;
			}
			return (CBManager.CBImpressionError)num;
		}
	}
}
