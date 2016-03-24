using GameMessage;
using SERVICE;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NrUserDeviceInfo : NrTSingleton<NrUserDeviceInfo>
{
	private AndroidJavaObject AndroidPlugin;

	private string strMoviePackageName = string.Empty;

	private NrUserDeviceInfo()
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		if (androidJavaClass != null)
		{
			this.AndroidPlugin = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
		}
		androidJavaClass.Dispose();
		this.strMoviePackageName = "com.ndoors.playmovie.VideoManager";
	}

	public string Getimei()
	{
		return this.AndroidPlugin.Call<string>("Getimei", new object[0]);
	}

	public string Getimsi()
	{
		return this.AndroidPlugin.Call<string>("Getimsi", new object[0]);
	}

	public string GetIPAddress()
	{
		return this.AndroidPlugin.CallStatic<string>("getLocalIpAddress", new object[]
		{
			1
		});
	}

	public string GetAppVersion()
	{
		return this.AndroidPlugin.Call<string>("GetAppVersion", new object[0]);
	}

	public string GetOSVersion()
	{
		return this.AndroidPlugin.Call<string>("GetOSversion", new object[0]);
	}

	public string GetCountry()
	{
		return this.AndroidPlugin.Call<string>("GetCountry", new object[0]);
	}

	public string GetMacAddress()
	{
		string text = this.AndroidPlugin.Call<string>("GetMacAddress", new object[0]);
		return text.Replace(":", string.Empty);
	}

	public string GetModelName()
	{
		return SystemInfo.deviceModel;
	}

	public string GetBrandName()
	{
		return this.AndroidPlugin.Call<string>("GetManufturer", new object[0]);
	}

	public void SaveHotKey(int Value)
	{
		this.AndroidPlugin.Call("SaveHotKey", new object[]
		{
			Value
		});
	}

	public int GetHotKey()
	{
		return 0;
	}

	public bool IsConnetInternet()
	{
		return this.AndroidPlugin.Call<bool>("checkInternetConnection", new object[]
		{
			this.AndroidPlugin
		});
	}

	public void TestCode()
	{
		Debug.Log(string.Format("[ip={0}] [ApkVersion={1}] [OSVersion={2}] [Country={3}] [MacAddress={4}] [imei={5}] [imsi={6}] [ModelName={7}] [BrandName={8}]", new object[]
		{
			this.GetIPAddress(),
			this.GetAppVersion(),
			this.GetOSVersion(),
			this.GetCountry(),
			this.GetMacAddress(),
			this.Getimei(),
			this.Getimsi(),
			this.GetModelName(),
			this.GetBrandName()
		}));
	}

	public void PlayMovieAtPath(string path, float fVolume = 1f)
	{
		MsgHandler.Handle("PlayMovieTime", new object[0]);
	}

	public void PlayMovieURL(string url, Color bgColor, bool bShowToast = true, float fVolum = 1f, bool isSkip = true)
	{
		Debug.Log("PlayMovieUrl : " + url);
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass(this.strMoviePackageName))
		{
			androidJavaClass.CallStatic("PlayMovieURL", new object[]
			{
				url,
				bShowToast,
				fVolum
			});
		}
		MsgHandler.Handle("PlayMovieTime", new object[0]);
	}

	public void SetMovieText(string txt)
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass(this.strMoviePackageName))
		{
			androidJavaClass.CallStatic("SetMovieText", new object[]
			{
				txt
			});
		}
	}

	public void BillingMode(bool bMode)
	{
		eSERVICE_AREA currentServiceArea = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();
		if (currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_KORTSTORE)
		{
			this.AndroidPlugin.Call("BillingMode", new object[]
			{
				bMode
			});
		}
	}

	public string GetPackageName()
	{
		string result = string.Empty;
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				result = @static.CallStatic<string>("GetPackageName", new object[]
				{
					@static
				});
			}
		}
		return result;
	}

	public void SetIncludeEmulator(List<string> Data)
	{
		this.AndroidPlugin.Call("SetIncludeEmulator", new object[]
		{
			Data.ToArray()
		});
	}

	public void SetActiveEmulator(List<string> Data)
	{
		this.AndroidPlugin.Call("SetActiveEmulator", new object[]
		{
			Data.ToArray()
		});
	}

	public void SetExceptionEmulator(List<string> Data)
	{
		this.AndroidPlugin.Call("SetExcptionEmulator", new object[]
		{
			Data.ToArray()
		});
	}

	public string isMacro()
	{
		string result;
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				result = @static.CallStatic<string>("isMacro", new object[]
				{
					@static
				});
			}
		}
		return result;
	}

	public bool isEmulator()
	{
		bool result;
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				result = @static.CallStatic<bool>("isEmulator", new object[]
				{
					@static
				});
			}
		}
		return result;
	}

	public bool isEmulatorTest()
	{
		bool result;
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				result = @static.CallStatic<bool>("isEmulatorTest", new object[0]);
			}
		}
		return result;
	}
}
