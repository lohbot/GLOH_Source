using System;
using UnityEngine;

public static class NrNewOpenURL
{
	public static string Help_WebHomeUrl = "http://tkgameweb.ndoors.com/dic/default.aspx?m=he";

	public static string Help_WebIdxUrl = NrNewOpenURL.Help_WebHomeUrl + "&idx=";

	public static int Help_Web_Width = 800;

	public static int Help_Web_Height = 600;

	public static void NewOpenHelp_URL()
	{
		Application.ExternalCall("OpenNewPopUpUrl", new object[]
		{
			NrNewOpenURL.Help_WebHomeUrl,
			NrNewOpenURL.Help_Web_Width,
			NrNewOpenURL.Help_Web_Height
		});
	}

	public static void NewOpenHelp_URL(string _Key)
	{
		Application.ExternalCall("OpenNewPopUpUrl", new object[]
		{
			NrNewOpenURL.Help_WebIdxUrl + _Key,
			NrNewOpenURL.Help_Web_Width,
			NrNewOpenURL.Help_Web_Height
		});
	}

	public static void NewOpenHelp_URL(string _Key, int Web_Width, int Web_Height)
	{
		Application.ExternalCall("OpenNewPopUpUrl", new object[]
		{
			NrNewOpenURL.Help_WebIdxUrl + _Key,
			Web_Width,
			Web_Height
		});
	}
}
