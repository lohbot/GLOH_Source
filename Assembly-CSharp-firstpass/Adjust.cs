using com.adjust.sdk;
using omniata;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Adjust : MonoBehaviour
{
	public const string sdkPrefix = "unity3.3.0";

	private static IAdjust instance;

	private static string errorMessage = "adjust: SDK not started. Start it manually using the 'appDidLaunch' method";

	private static Action<ResponseData> responseDelegate;

	public string appToken = "{Your App Token}";

	public AdjustUtil.LogLevel logLevel = AdjustUtil.LogLevel.Info;

	public AdjustUtil.AdjustEnvironment environment;

	public bool eventBuffering;

	public bool startManually;

	private void Awake()
	{
		if (!this.startManually)
		{
			this.environment = AdjustUtil.AdjustEnvironment.Production;
			string text = string.Empty;
			if (Application.platform != RuntimePlatform.Android)
			{
				return;
			}
			text = "ps77gcz96hls";
			Adjust.appDidLaunch(text, this.environment, this.logLevel, this.eventBuffering);
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (Adjust.instance == null)
		{
			return;
		}
		if (pauseStatus)
		{
			Adjust.instance.onPause();
		}
		else
		{
			Adjust.instance.onResume();
		}
	}

	public static void appDidLaunch(string appToken, AdjustUtil.AdjustEnvironment environment, AdjustUtil.LogLevel logLevel, bool eventBuffering)
	{
		if (Adjust.instance != null)
		{
			Debug.Log("adjust: warning, SDK already started. Restarting");
		}
		Adjust.instance = new AdjustAndroid();
		if (Adjust.instance == null)
		{
			Debug.Log("adjust: SDK can only be used in Android, iOS, Windows Phone 8 or Windows Store apps");
			return;
		}
		Adjust.instance.appDidLaunch(appToken, environment, "unity3.3.0", logLevel, eventBuffering);
		GameObject gameObject = GameObject.Find("OmniataManager");
		if (gameObject == null)
		{
			return;
		}
		OmniataComponent component = gameObject.GetComponent<OmniataComponent>();
		if (component == null)
		{
			return;
		}
		Omniata omniataInstance = component.GetOmniataInstance();
		if (omniataInstance == null)
		{
			return;
		}
		if (!omniataInstance.Initialized)
		{
			return;
		}
		string eventToken = string.Empty;
		eventToken = "fyn6ku";
		Adjust.trackEvent(eventToken, new Dictionary<string, string>
		{
			{
				"user",
				omniataInstance.UserID
			},
			{
				"api_key",
				omniataInstance.ApiKey
			}
		});
	}

	public static void trackEvent(string eventToken, Dictionary<string, string> parameters = null)
	{
		if (Adjust.instance == null)
		{
			Debug.Log(Adjust.errorMessage);
			return;
		}
		Adjust.instance.trackEvent(eventToken, parameters);
	}

	public static void trackRevenue(double cents, string eventToken = null, Dictionary<string, string> parameters = null)
	{
		if (Adjust.instance == null)
		{
			Debug.Log(Adjust.errorMessage);
			return;
		}
		Adjust.instance.trackRevenue(cents, eventToken, parameters);
	}

	public static void setResponseDelegate(Action<ResponseData> responseDelegate, string sceneName = "Adjust")
	{
		if (Adjust.instance == null)
		{
			Debug.Log(Adjust.errorMessage);
			return;
		}
		Adjust.responseDelegate = responseDelegate;
		Adjust.instance.setResponseDelegate(sceneName);
		Adjust.instance.setResponseDelegateString(new Action<string>(Adjust.runResponseDelegate));
	}

	public static void setEnabled(bool enabled)
	{
		if (Adjust.instance == null)
		{
			Debug.Log(Adjust.errorMessage);
			return;
		}
		Adjust.instance.setEnabled(enabled);
	}

	public static bool isEnabled()
	{
		if (Adjust.instance == null)
		{
			Debug.Log(Adjust.errorMessage);
			return false;
		}
		return Adjust.instance.isEnabled();
	}

	public void getNativeMessage(string sResponseData)
	{
		Adjust.runResponseDelegate(sResponseData);
	}

	public static void runResponseDelegate(string sResponseData)
	{
		if (Adjust.instance == null)
		{
			Debug.Log(Adjust.errorMessage);
			return;
		}
		if (Adjust.responseDelegate == null)
		{
			Debug.Log("adjust: response delegate not set to receive callbacks");
			return;
		}
		ResponseData obj = new ResponseData(sResponseData);
		Adjust.responseDelegate(obj);
	}
}
