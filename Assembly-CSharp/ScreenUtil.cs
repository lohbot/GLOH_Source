using System;
using UnityEngine;

public class ScreenUtil
{
	public static float ScreenWidthToUIWidth(float fScreenWidth)
	{
		float num = 1f / (float)Screen.width * fScreenWidth;
		return GUICamera.width * num;
	}

	public static float UIWidthToScreenWidth(float fUIWidth)
	{
		float num = 1f / GUICamera.width * fUIWidth;
		return (float)Screen.width * num;
	}

	public static float ScreenHeightToUIHeight(float fScreenHeight)
	{
		float num = 1f / (float)Screen.height * fScreenHeight;
		return GUICamera.width * num;
	}

	public static float UIWidthToScreenHeight(float fUIHeight)
	{
		float num = 1f / GUICamera.height * fUIHeight;
		return (float)Screen.width * num;
	}

	public static float GUIScreenToPercentWithControlX(float _ControlSize, float _Percent)
	{
		return (GUICamera.width - _ControlSize) * (_Percent / 100f);
	}

	public static float ScreenToPercentWithControlX(float _ControlSize, float _Percent)
	{
		return ((float)Screen.width - _ControlSize) * (_Percent / 100f);
	}

	public static float GUIScreenToPercentX(float _Percent)
	{
		return GUICamera.width * (_Percent / 100f);
	}

	public static int GUIScreenToPercentY(float _Percent)
	{
		return (int)(GUICamera.height * (_Percent / 100f));
	}
}
