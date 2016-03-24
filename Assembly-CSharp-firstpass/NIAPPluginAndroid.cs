using System;
using UnityEngine;

public class NIAPPluginAndroid : NIAPPlugin
{
	public AndroidJavaObject activity;

	public NIAPPluginAndroid()
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		this.activity = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
	}

	public override void invoke(NIAPParam param)
	{
		this.activity.Call("callNIAPNativeExtension", new object[]
		{
			param.toString()
		});
	}

	public override void showMessage(string message)
	{
		this.activity.Call("showMessage", new object[]
		{
			message
		});
	}
}
