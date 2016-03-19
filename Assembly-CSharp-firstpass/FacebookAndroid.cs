using Prime31;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FacebookAndroid
{
	private static AndroidJavaObject _facebookPlugin;

	static FacebookAndroid()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.prime31.FacebookPlugin"))
		{
			FacebookAndroid._facebookPlugin = androidJavaClass.CallStatic<AndroidJavaObject>("instance", new object[0]);
		}
		FacebookManager.preLoginSucceededEvent += delegate
		{
			Facebook.instance.accessToken = FacebookAndroid.getAccessToken();
		};
	}

	internal static void babysitRequest(bool requiresPublishPermissions, Action afterAuthAction)
	{
		new FacebookAuthHelper(requiresPublishPermissions, afterAuthAction).start();
	}

	public static void init(bool printKeyHash = true)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		FacebookAndroid._facebookPlugin.Call("init", new object[]
		{
			printKeyHash
		});
		Facebook.instance.accessToken = FacebookAndroid.getAccessToken();
	}

	public static string getAppLaunchUrl()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return string.Empty;
		}
		return FacebookAndroid._facebookPlugin.Call<string>("getAppLaunchUrl", new object[0]);
	}

	public static void setSessionLoginBehavior(FacebookSessionLoginBehavior loginBehavior)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		FacebookAndroid._facebookPlugin.Call("setSessionLoginBehavior", new object[]
		{
			loginBehavior.ToString()
		});
	}

	public static bool isSessionValid()
	{
		return Application.platform == RuntimePlatform.Android && FacebookAndroid._facebookPlugin.Call<bool>("isSessionValid", new object[0]);
	}

	public static string getAccessToken()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return string.Empty;
		}
		return FacebookAndroid._facebookPlugin.Call<string>("getAccessToken", new object[0]);
	}

	public static List<object> getSessionPermissions()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			string json = FacebookAndroid._facebookPlugin.Call<string>("getSessionPermissions", new object[0]);
			return json.listFromJson();
		}
		return new List<object>();
	}

	public static void login()
	{
		FacebookAndroid.loginWithReadPermissions(new string[0]);
	}

	public static void loginWithReadPermissions(string[] permissions)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		FacebookAndroid._facebookPlugin.Call("loginWithReadPermissions", new object[]
		{
			permissions
		});
	}

	public static void loginWithPublishPermissions(string[] permissions)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		FacebookAndroid._facebookPlugin.Call("loginWithPublishPermissions", new object[]
		{
			permissions
		});
	}

	public static void reauthorizeWithReadPermissions(string[] permissions)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		FacebookAndroid._facebookPlugin.Call("reauthorizeWithReadPermissions", new object[]
		{
			permissions.toJson()
		});
	}

	public static void reauthorizeWithPublishPermissions(string[] permissions, FacebookSessionDefaultAudience defaultAudience)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		string text;
		if (defaultAudience != FacebookSessionDefaultAudience.OnlyMe)
		{
			text = defaultAudience.ToString().ToUpper();
		}
		else
		{
			text = "ONLY_ME";
		}
		FacebookAndroid._facebookPlugin.Call("reauthorizeWithPublishPermissions", new object[]
		{
			permissions.toJson(),
			text
		});
	}

	public static void logout()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		FacebookAndroid._facebookPlugin.Call("logout", new object[0]);
		Facebook.instance.accessToken = string.Empty;
	}

	public static void showFacebookShareDialog(Dictionary<string, object> parameters)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		FacebookAndroid._facebookPlugin.Call("showFacebookShareDialog", new object[]
		{
			parameters.toJson()
		});
	}

	public static void showDialog(string dialogType, Dictionary<string, string> parameters)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		parameters = (parameters ?? new Dictionary<string, string>());
		if (!FacebookAndroid.isSessionValid())
		{
			FacebookAndroid.babysitRequest(false, delegate
			{
				FacebookAndroid._facebookPlugin.Call("showDialog", new object[]
				{
					dialogType,
					parameters.toJson()
				});
			});
		}
		else
		{
			FacebookAndroid._facebookPlugin.Call("showDialog", new object[]
			{
				dialogType,
				parameters.toJson()
			});
		}
	}

	public static void graphRequest(string graphPath, string httpMethod, Dictionary<string, string> parameters)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		parameters = (parameters ?? new Dictionary<string, string>());
		if (!FacebookAndroid.isSessionValid())
		{
			FacebookAndroid.babysitRequest(true, delegate
			{
				FacebookAndroid._facebookPlugin.Call("graphRequest", new object[]
				{
					graphPath,
					httpMethod,
					parameters.toJson()
				});
			});
		}
		else
		{
			FacebookAndroid._facebookPlugin.Call("graphRequest", new object[]
			{
				graphPath,
				httpMethod,
				parameters.toJson()
			});
		}
	}

	public static void setAppVersion(string version)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		FacebookAndroid._facebookPlugin.Call("setAppVersion", new object[]
		{
			version
		});
	}

	public static void logEvent(string eventName, Dictionary<string, object> parameters = null)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		if (parameters != null)
		{
			FacebookAndroid._facebookPlugin.Call("logEventWithParameters", new object[]
			{
				eventName,
				Json.encode(parameters)
			});
		}
		else
		{
			FacebookAndroid._facebookPlugin.Call("logEvent", new object[]
			{
				eventName
			});
		}
	}

	public static void logEvent(string eventName, double valueToSum, Dictionary<string, object> parameters = null)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		if (parameters != null)
		{
			FacebookAndroid._facebookPlugin.Call("logEventAndValueToSumWithParameters", new object[]
			{
				eventName,
				valueToSum,
				Json.encode(parameters)
			});
		}
		else
		{
			FacebookAndroid._facebookPlugin.Call("logEventAndValueToSum", new object[]
			{
				eventName,
				valueToSum
			});
		}
	}
}
