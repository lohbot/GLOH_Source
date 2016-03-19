using BUILD;
using System;
using UnityEngine;

public class NrVersionController
{
	public static void Init()
	{
		int mVersion = LoHBuildInfo.mVersion;
		int @int = PlayerPrefs.GetInt(LoHBuildInfo.VERSION_REG_KEY);
		if (@int != mVersion)
		{
			PlayerPrefs.SetInt(LoHBuildInfo.VERSION_REG_KEY, mVersion);
		}
	}

	public static void OnVersion()
	{
		Vector2 vector = default(Vector2);
		Color color = GUI.color;
		GUI.color = new Color(0f, 255f, 0f);
		float num = 200f;
		vector.x = (float)Screen.width - num;
		GUI.Label(new Rect(vector.x, vector.y += 20f, num, 20f), "Version:[" + LoHBuildInfo.mVersion + "]");
		GUI.Label(new Rect(vector.x, vector.y += 20f, num, 20f), "SVN Time:[" + LoHBuildInfo.mLastCommitTime + "]");
		GUI.Label(new Rect(vector.x, vector.y += 20f, num, 20f), "Build Time[" + LoHBuildInfo.mBuildingTime + "]");
		GUI.color = color;
	}
}
