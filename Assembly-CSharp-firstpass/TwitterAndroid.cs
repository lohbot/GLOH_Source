using Prime31;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TwitterAndroid
{
	private static AndroidJavaObject _plugin;

	static TwitterAndroid()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.prime31.TwitterPlugin"))
		{
			TwitterAndroid._plugin = androidJavaClass.CallStatic<AndroidJavaObject>("instance", new object[0]);
		}
	}

	public static void init(string consumerKey, string consumerSecret)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		TwitterAndroid._plugin.Call("init", new object[]
		{
			consumerKey,
			consumerSecret
		});
	}

	public static bool isLoggedIn()
	{
		return Application.platform == RuntimePlatform.Android && TwitterAndroid._plugin.Call<bool>("isLoggedIn", new object[0]);
	}

	public static void showLoginDialog()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		TwitterAndroid._plugin.Call("showLoginDialog", new object[0]);
	}

	public static void logout()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		TwitterAndroid._plugin.Call("logout", new object[0]);
	}

	public static void postStatusUpdate(string status)
	{
		Dictionary<string, string> parameters = new Dictionary<string, string>
		{
			{
				"status",
				status
			}
		};
		TwitterAndroid.performRequest("post", "/1.1/statuses/update.json", parameters);
	}

	public static void postStatusUpdate(string update, byte[] image)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		TwitterAndroid._plugin.Call("postUpdateWithImage", new object[]
		{
			update,
			image
		});
	}

	public static void getHomeTimeline()
	{
		TwitterAndroid.performRequest("get", "/1.1/statuses/home_timeline.json", null);
	}

	public static void getFollowers()
	{
		TwitterAndroid.performRequest("get", "/1.1/statuses/followers.json", null);
	}

	public static void performRequest(string methodType, string path, Dictionary<string, string> parameters)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		string text = (parameters == null) ? string.Empty : parameters.toJson();
		TwitterAndroid._plugin.Call("performRequest", new object[]
		{
			methodType,
			path,
			text
		});
	}
}
