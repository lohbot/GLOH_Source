using Prime31;
using SERVICE;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AdWords : NrTSingleton<AdWords>
{
	private AndroidJavaObject AndroidPlugin;

	private eSERVICE_AREA eCurrentService = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();

	private AdWords()
	{
		if (this.eCurrentService == eSERVICE_AREA.SERVICE_ANDROID_KORGOOGLE || this.eCurrentService == eSERVICE_AREA.SERVICE_ANDROID_KORQA)
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			if (androidJavaClass != null)
			{
				this.AndroidPlugin = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			}
			androidJavaClass.Dispose();
		}
	}

	public void CreateCharacterComplete()
	{
		if (this.AndroidPlugin != null)
		{
			this.AndroidPlugin.Call("CreateCharacterComplete", new object[0]);
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("action_type", "subscribe");
			this.AndroidPlugin.Call("RemarKetingData", new object[]
			{
				dictionary.toJson()
			});
		}
	}

	public void SalesData(string Value)
	{
		if (this.AndroidPlugin != null)
		{
			this.AndroidPlugin.Call("SalesData", new object[]
			{
				Value
			});
		}
	}

	public void PurchaseData(ITEM_MALL_ITEM pData)
	{
	}

	public void LevelUp(int nLevel)
	{
		if (this.AndroidPlugin != null)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("action_type", "complete_level");
			dictionary.Add("value", nLevel.ToString());
			this.AndroidPlugin.Call("RemarKetingData", new object[]
			{
				dictionary.toJson()
			});
		}
	}
}
