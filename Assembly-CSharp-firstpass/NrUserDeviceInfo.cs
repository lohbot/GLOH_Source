using System;
using UnityEngine;

public class NrUserDeviceInfo : NrTSingleton<NrUserDeviceInfo>
{
	private AndroidJavaObject AndroidPlugin;

	private NrUserDeviceInfo()
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		if (androidJavaClass != null)
		{
			this.AndroidPlugin = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
		}
		androidJavaClass.Dispose();
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
}
