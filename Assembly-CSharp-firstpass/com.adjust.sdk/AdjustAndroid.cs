using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.adjust.sdk
{
	public class AdjustAndroid : IAdjust
	{
		private AndroidJavaClass ajcAdjust;

		private AndroidJavaClass ajcAdjustUnity;

		private AndroidJavaObject ajoCurrentActivity;

		public AdjustAndroid()
		{
			this.ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
			this.ajcAdjustUnity = new AndroidJavaClass("com.adjust.sdk.AdjustUnity");
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			this.ajoCurrentActivity = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
		}

		public void appDidLaunch(string appToken, AdjustUtil.AdjustEnvironment environment, string sdkPrefix, AdjustUtil.LogLevel logLevel, bool eventBuffering)
		{
			string text = environment.ToString().ToLower();
			string text2 = logLevel.ToString().ToLower();
			this.ajcAdjust.CallStatic("appDidLaunch", new object[]
			{
				this.ajoCurrentActivity,
				appToken,
				text,
				text2,
				eventBuffering
			});
			this.ajcAdjust.CallStatic("setSdkPrefix", new object[]
			{
				sdkPrefix
			});
			this.onResume();
		}

		public void trackEvent(string eventToken, Dictionary<string, string> parameters = null)
		{
			AndroidJavaObject androidJavaObject = this.ConvertDicToJava(parameters);
			this.ajcAdjust.CallStatic("trackEvent", new object[]
			{
				eventToken,
				androidJavaObject
			});
		}

		public void trackRevenue(double cents, string eventToken = null, Dictionary<string, string> parameters = null)
		{
			AndroidJavaObject androidJavaObject = this.ConvertDicToJava(parameters);
			this.ajcAdjust.CallStatic("trackRevenue", new object[]
			{
				cents,
				eventToken,
				androidJavaObject
			});
		}

		public void onPause()
		{
			this.ajcAdjust.CallStatic("onPause", new object[0]);
		}

		public void onResume()
		{
			this.ajcAdjust.CallStatic("onResume", new object[]
			{
				this.ajoCurrentActivity
			});
		}

		public void setResponseDelegate(string sceneName)
		{
			this.ajcAdjustUnity.CallStatic("setResponseDelegate", new object[]
			{
				sceneName
			});
		}

		public void setResponseDelegateString(Action<string> responseDelegate)
		{
		}

		public void setEnabled(bool enabled)
		{
			this.ajcAdjust.CallStatic("setEnabled", new object[]
			{
				this.ConvertBoolToJava(enabled)
			});
		}

		public bool isEnabled()
		{
			AndroidJavaObject ajo = this.ajcAdjust.CallStatic<AndroidJavaObject>("isEnabled", new object[0]);
			bool? flag = this.ConvertBoolFromJava(ajo);
			return flag.HasValue && flag.Value;
		}

		private AndroidJavaObject ConvertBoolToJava(bool value)
		{
			return new AndroidJavaObject("java.lang.Boolean", new object[]
			{
				value.ToString().ToLower()
			});
		}

		private bool? ConvertBoolFromJava(AndroidJavaObject ajo)
		{
			if (ajo == null)
			{
				return null;
			}
			string value = ajo.Call<string>("toString", new object[0]);
			bool? result;
			try
			{
				result = new bool?(Convert.ToBoolean(value));
			}
			catch (FormatException)
			{
				result = null;
			}
			return result;
		}

		private AndroidJavaObject ConvertDicToJava(Dictionary<string, string> dictonary)
		{
			if (dictonary == null)
			{
				return null;
			}
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.HashMap", new object[]
			{
				dictonary.Count
			});
			foreach (KeyValuePair<string, string> current in dictonary)
			{
				if (current.Value != null)
				{
					androidJavaObject.Call<string>("put", new object[]
					{
						current.Key,
						current.Value
					});
				}
			}
			return androidJavaObject;
		}
	}
}
