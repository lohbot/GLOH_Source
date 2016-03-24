using System;
using UnityEngine;

public class ScreenSizeManager : NrTSingleton<ScreenSizeManager>
{
	private ScreenSizeManager()
	{
	}

	public bool IsScreen4_3()
	{
		float num = (float)Screen.width / (float)Screen.height;
		return 1.3f < num && num < 1.4f;
	}
}
